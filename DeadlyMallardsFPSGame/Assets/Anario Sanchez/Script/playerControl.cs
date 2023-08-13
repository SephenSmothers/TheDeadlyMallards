using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditorInternal;
using UnityEngine;
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
    [SerializeField] float stamina;
    [SerializeField] float jumpHeight;

    private float maxStamina;
    Vector3 move;
    private float playerSpeed;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float origFOV;
    private bool tired;
    public int maxHP;
    public GameObject pointer;
   


    public MovementState state;

    public enum MovementState
    {
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
        spawnPlayer();
    }
    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (GameManager.instance._activeMenu == null)
        {
            stateCheck();
            movement();
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
            playerSpeed = playerWalkSpeed / 2;
            bobbing.speed = 2;
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView,55,10f * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftShift) && !tired)
        {
            state = MovementState.sprinting;
            staminaRunning();
            playerSpeed = playerSprintSpeed;
            bobbing.speed = 8;
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, 70, 10f * Time.deltaTime);
        }
        else if (groundedPlayer)
        {
            state = MovementState.walking;
            staminaRegen();
            playerSpeed = playerWalkSpeed;
            bobbing.speed = 4;
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, origFOV, 10f * Time.deltaTime);
        }
        else
        {
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
            stamina += 1 * Time.deltaTime;
        }
        else
        {
            tired = false;
        }

    }
    public void CanTakeDamage(int amount)
    {
        hp -= amount;


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
        if (hp < 10) 
        {
            StartCoroutine(GameManager.instance.FlashScreen());
        }
        if (hp <= 0)
        {
            GameManager.instance.YoLose();
        }

    }

    public void GetMaxHealth()
    {
        hp = maxHP;
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
}
