using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Monaghan, Devin
/// 5/7/2024
/// Handles deathfloor
/// handles WASD movement
/// handles jumping
/// handles boosts
/// </summary>

public class PlayerController : MonoBehaviour
{
    // speed variables
    public float walkSpeed = 15f;
    public float sprintSpeed = 50f;
    public float floatSpeed = 20f;
    // gravity speed
    public float gravitySpeed = 20f;
    // power of jump
    public float jumpForce = 20f;

    // is the player actively jumping
    public bool floating = false;
    // is the player on the ground
    public bool onGround = true;
    // is the player sprinting or walking
    public bool sprinting = false;
    // is the player actively moving;
    public bool moving = false;

    // reference to treads model
    public GameObject treads;

    // reference to inputs
    private PlayerInputActions playerInputActions;
    // reference to rigidbody
    private Rigidbody rigidBodyRef;

    // Awake is called before the first frame update
    void Awake()
    {
        // get rigidbody reference
        rigidBodyRef = this.GetComponent<Rigidbody>();

        // get inputs
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // disallow the player to fall off the map
        Deathfloor();
        // check if the player is on the ground or not
        OnGround();
    }

    // handles physics controlled movement
    private void FixedUpdate()
    {
        // move
        Move();
        // jump and float
        Jump();
        // player falls in air
        Gravity();
        // slow down when not moving
        SlowDown();
    }

    // get movement inputs
    // move via applying force to rigidbody
    private void Move()
    {
        // get movement input values
        Vector2 vectorWASD = playerInputActions.PlayerActions.MoveWASD.ReadValue<Vector2>();
        // convert Vector2 inputs into a Vector3
        Vector3 direction = Vector3.zero;
        direction.x = vectorWASD.x;
        direction.z = vectorWASD.y;

        // apply force
        if (Sprinting())
        {
            rigidBodyRef.AddForce(direction * sprintSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
        }
        else
        {
            rigidBodyRef.AddForce(direction * walkSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }

    // when player is not moving rapidly slow down
    private void SlowDown()
    {
        if (!Moving() && rigidBodyRef.velocity != Vector3.zero)
        {
            rigidBodyRef.velocity /= 1.03f;
        }
    }

    // get sprint input
    // when player starts sprinting enter sprinting state
    // when player stops inputting movements exit sprinting state
    // return true in springting state and false out of sprinting state
    private bool Moving()
    {
        // get movement input
        Vector2 vectorWASD = playerInputActions.PlayerActions.MoveWASD.ReadValue<Vector2>();

        // if player starts moving enter moving state
        if (vectorWASD != Vector2.zero)
        {
            moving = true;
        }
        // if player stops movement exit moving state
        else
        {
            moving = false;
        }

        return moving;
    }

    // get sprint input
    // when player starts sprinting enter sprinting state
    // when player stops inputting movements exit sprinting state
    // return true in springting state and false out of sprinting state
    private bool Sprinting()
    {
        // get movement input
        Vector2 vectorWASD = playerInputActions.PlayerActions.MoveWASD.ReadValue<Vector2>();

        // if player presses ctrl start sprinting
        if (playerInputActions.PlayerActions.Sprint.WasPerformedThisFrame())
        {
            sprinting = true;
        }
        // if player stops movement stop sprinting
        if (vectorWASD == Vector2.zero)
        {
            sprinting = false;
        }

        return sprinting;
    }

    // check if player is on the ground via raycast
    private void OnGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.6f))
        {
            onGround = true;
            Debug.DrawLine(transform.position, new Vector3(transform.position.x, -1f * hit.distance, transform.position.z),
                Color.red);
        }
        else
        {
            onGround = false;
            Debug.DrawLine(transform.position, new Vector3(transform.position.x, -1f * hit.distance, transform.position.z),
                Color.red);
        }
    }

    // if the player is not on the ground or jumping apply force down
    private void Gravity()
    {
        if (!floating && !onGround)
        {
            rigidBodyRef.AddForce(Vector3.down * jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }

    // get jump inputs
    // if player presses space apply a force up
    // if player keeps pressing space float via continuous 
    private void Jump()
    {
        // if the player is on the ground and presses the spacebar, jump
        if (onGround && playerInputActions.PlayerActions.Jump.WasPerformedThisFrame())
        {
            rigidBodyRef.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        // if the player is not on the ground and holds space bar, continuosly provide small lift
        if (playerInputActions.PlayerActions.Jump.IsPressed())
        {
            rigidBodyRef.AddForce(Vector3.up * floatSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
            floating = true;
        }
        else
        {
            floating = false;
        }
    }

    // checks if player has fallen off the floor and teleports them back to origin, resetting force
    private void Deathfloor()
    {
        if (transform.position.y <= -15)
        {
            // reset position to origin
            transform.position = new Vector3(0f, 0f, 0f);
            // reset rotation
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            // reset momentum
            rigidBodyRef.velocity = Vector3.zero;
            print("player died");
        }
    }
}