using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class playerControl : MonoBehaviour, TakeDamage
{
    [Header("----- Compents -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Camera playerCam;
    [SerializeField] headBobbing bobbing;

    [Header("----- Player Stats -----")]
    public int hp;
    [SerializeField] float playerWalkSpeed;
    [SerializeField] float playerSprintSpeed;
    public float stamina;
    [SerializeField] float jumpHeight;

    public float maxStamina;
    Vector3 move;
    private float playerSpeed;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float origFOV;
    private bool tired;
    public int maxHP;
    public Transform gun;
    public Transform ads;
    public Transform origGun;
    public GameObject pointer;
    public MovementState state;
    public bool isBossPlayer;

    public float invulnerabilityDuration = .5f;
    private float invulnerabilityTimer = 0.0f;
    public bool isInvulnerable = false;
    private bool safety;

    public float playerVel;
    public enum MovementState
    {
        ads,
        exhausted,
        walking,
        sprinting,
        air
    }
    private void Start()
    {
        origFOV = playerCam.fieldOfView;
        maxStamina = stamina;
        maxHP = hp;
        safety = true;
        spawnPlayer();
    }
    void Update()
    {
        playerVel = playerVelocity.y;
        groundedPlayer = controller.isGrounded;
        if (GameManager.instance._activeMenu == null)
        {
            stateCheck();
            movement();
        }
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer <= 0.0f)
            {
                isInvulnerable = false;
            }
        }
        if (GameManager.instance.shootingScript.gunList[GameManager.instance.shootingScript.selectedGun].gunName == "Revolver")
        {
            gun.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else
        {
            gun.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        }
    }

    void movement()
    {
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        move = (transform.right * Input.GetAxis("Horizontal")) +
                (transform.forward * Input.GetAxis("Vertical"));

        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (Input.GetButton("Jump") && groundedPlayer)
        {
            playerVelocity.y = jumpHeight;
        }
        playerVelocity.y -= 9.81f * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    void stateCheck()
    {
        if (tired)
        {
            state = MovementState.exhausted;
            staminaRegen();
            resetSpread();
            playerSpeed = playerWalkSpeed / 2;
            bobbing.speed = 2;
            gun.position = Vector3.Lerp(gun.position, origGun.position, 10f * Time.deltaTime);
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, 55, 10f * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Mouse1) && !GameManager.instance.shootingScript.reloading)
        {
            state = MovementState.ads;
            staminaRegen();
            playerSpeed = playerWalkSpeed / 2;
            bobbing.intensity = 0f;
            if (GameManager.instance.shootingScript.gunList[GameManager.instance.shootingScript.selectedGun].gunName == "Shotgun")
            {
                GameManager.instance.shootingScript.gunList[GameManager.instance.shootingScript.selectedGun].spread = 0.05f;
            }
            else
            {
                GameManager.instance.shootingScript.gunList[GameManager.instance.shootingScript.selectedGun].spread = 0f;
            }
            GameManager.instance.shootingScript.anim.SetBool("Ads", true);
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, 35, 10f * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftShift) && !tired && state != MovementState.ads)
        {
            state = MovementState.sprinting;
            staminaRunning();
            resetSpread();
            playerSpeed = playerSprintSpeed;
            bobbing.speed = 8;
            bobbing.intensity = 0.02f;
            gun.position = Vector3.Lerp(gun.position, origGun.position, 10f * Time.deltaTime);
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, 70, 10f * Time.deltaTime);
        }
        else if (groundedPlayer)
        {
            state = MovementState.walking;
            staminaRegen();
            resetSpread();
            playerSpeed = playerWalkSpeed;
            bobbing.speed = 4;
            bobbing.intensity = 0.02f;
            gun.position = Vector3.Lerp(gun.position, origGun.position, 10f * Time.deltaTime);
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, origFOV, 10f * Time.deltaTime);
        }
        else
        {
            resetSpread();
            state = MovementState.air;
        }
    }
    void staminaRunning()
    {
        if (stamina >= 0)
        {
            stamina -= 1 * Time.deltaTime;
        }
        else
        {
            tired = true;
        }
    }
    void staminaRegen()
    {
        if (stamina <= maxStamina)
        {
            stamina += 2 * Time.deltaTime;
        }
        else
        {
            tired = false;
        }
    }
    public void CanTakeDamage(int amount)
    {
        if (!isInvulnerable)
        {
            hp -= amount;
            isInvulnerable = true;
            invulnerabilityTimer = invulnerabilityDuration;


            GameObject HitMarker = Instantiate(pointer, transform.position, Quaternion.identity);
            Vector3 directional = transform.position - HitMarker.transform.position;

            Quaternion rotation = Quaternion.LookRotation(directional);
            rotation.z = -rotation.y;
            rotation.y = 0;
            rotation.x = 0;
            Vector3 Northdirection = new Vector3(0, 0, transform.eulerAngles.y);
            GameManager.instance.damageIndicator.transform.localRotation = rotation * Quaternion.Euler(Northdirection);
            StartCoroutine(GameManager.instance.DamageDirection());

            Destroy(HitMarker, .5f);
            GameManager.instance.UpdatePlayerUI();
            ScoreManager.instance.UpdateTotalDamageTaken(amount);
            ScoreManager.instance.UpdateScores();
            if (hp < 10)
            {
                StartCoroutine(GameManager.instance.FlashScreen());
            }
            if (hp <= 0 && safety)
            {
                hp = 1;
                safety = false;
            }
            if (hp <= 0)
            {
                GameManager.instance.YoLose();
            }

        }

    }

    public void GetMaxHealth()
    {
        hp = maxHP;
        safety = true;
        GameManager.instance.UpdatePlayerUI();
    }
    void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = GameManager.instance._playerSpawn.transform.position;
        controller.enabled = true;
        hp = maxHP;
        GameManager.instance.UpdatePlayerUI();
    }

    public bool isTiredChecker()
    {
        return tired;
    }
    
    void resetSpread()
    {
        if (GameManager.instance.shootingScript.gunList[GameManager.instance.shootingScript.selectedGun].gunName == "Shotgun")
        {
            GameManager.instance.shootingScript.gunList[GameManager.instance.shootingScript.selectedGun].spread = GameManager.instance.shootingScript.gunList[GameManager.instance.shootingScript.selectedGun].origSpread;
        }
        else
        {
            GameManager.instance.shootingScript.gunList[GameManager.instance.shootingScript.selectedGun].spread = GameManager.instance.shootingScript.gunList[GameManager.instance.shootingScript.selectedGun].origSpread;
        }
        GameManager.instance.shootingScript.anim.SetBool("Ads", false);
    }
}
