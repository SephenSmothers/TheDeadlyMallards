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
    

    [Header("----- Player Stats -----")]
    public int hp;
    [SerializeField] float playerWalkSpeed;
    [SerializeField] float playerSprintSpeed;
    [SerializeField] float jumpHeight;
    Vector3 move;
    private float playerSpeed;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public int maxHP;
    public GameObject pointer;
  

    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        air
    }
    private void Start()
    {
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
        if (Input.GetKey(KeyCode.LeftShift) && groundedPlayer)
        {
            state = MovementState.sprinting;
            playerSpeed = playerSprintSpeed;
        }
        else if (groundedPlayer)
        {
            state = MovementState.walking;
            playerSpeed = playerWalkSpeed;
        }
        else
        {
            state = MovementState.air;
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
        if(hp < 10)
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
