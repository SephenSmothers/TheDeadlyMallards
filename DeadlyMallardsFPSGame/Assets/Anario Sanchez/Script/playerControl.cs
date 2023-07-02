using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class playerControl : MonoBehaviour, TakeDamage
{
    private bool groundedPlayer;
    private float playerSpeed;

    public Transform orientation;
    [Header("Player Stats")]
    [SerializeField] float playerWalkSpeed;
    [SerializeField] float playerSprintSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] int playerHp;
    public float groundDrag;
    public float airDrag;
    public float playerHeight;

    [Header("State Check")]
    public LayerMask whatIsGround;

    Vector3 move;
    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        groundedPlayer = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        stateCheck();
        movement();
        speedControl();

        if (Input.GetKeyDown(KeyCode.Space) && groundedPlayer)
        {
            Jump();
        }

        if (state == MovementState.walking || state == MovementState.sprinting)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    void movement()
    {
        move = (orientation.right * Input.GetAxisRaw("Horizontal")) + (orientation.forward * Input.GetAxisRaw("Vertical"));

        if (groundedPlayer)
        {
            rb.AddForce(move.normalized * playerSpeed, ForceMode.Force);
        }
        else if (!groundedPlayer)
        {
            rb.AddForce(move.normalized * playerSpeed * airDrag, ForceMode.Force);
        }

    }
    void stateCheck()
    {
        if (Input.GetKey(KeyCode.LeftShift) && groundedPlayer)
        {
            state = MovementState.sprinting;
            playerSpeed = playerSprintSpeed;
        }
        else if(groundedPlayer)
        {
            state = MovementState.walking;
            playerSpeed = playerWalkSpeed;
        }
        else
        {
            state = MovementState.air;
        }
    }
    private void speedControl()
    {
        Vector3 flatvel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatvel.magnitude > playerSpeed)
        {
            Vector3 limitvel = flatvel.normalized * playerSpeed;
            rb.velocity = new Vector3(limitvel.x, rb.velocity.y, limitvel.z);
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
    }

    public void CanTakeDamage(int amount)
    {
        playerHp -= amount;
        if (playerHp <= 0)
        {
            GameManager.instance.YoLose();
        }
    }
}
