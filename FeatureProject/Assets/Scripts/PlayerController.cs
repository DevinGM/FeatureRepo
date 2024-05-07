using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Monaghan, Devin
/// 4/23/2024
/// Handles deathfloor
/// </summary>

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    private Rigidbody rigidBodyRef;

    // direction
    private Vector3 direction = Vector3.zero;

    private float force;

    // is the player actively jumping
    private bool jumping = false;
    // is the player on the ground
    public bool onGround = true;

    // force applied with jump
    public float jumpForce = 5f;

    // Awake is called before the first frame update
    void Awake()
    {
        // get rigidbody reference
        rigidBodyRef = this.GetComponent<Rigidbody>();;
        // set default force
        force = 75f;

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
        // move via force
        Move();
    }

    // get inputs
    private void Inputs()
    {

        /*
        // get movement input values
        Vector2 vectorWASD = playerInputActions.PlayerActions.MoveWASD.ReadValue<Vector2>();
        // convert Vector2 inputs into a Vector3 direction
        direction.x = vectorWASD.x;
        direction.z = vectorWASD.y;
        
        // get jump input values
        Vector2 boost = playerInputActions.PlayerActions.Boost.ReadValue<Vector2>();
        */

    }

    // get movement inputs
    // use inputs to move via force
    private void Move()
    {
        // get movement input values
        Vector2 vectorWASD = playerInputActions.PlayerActions.MoveWASD.ReadValue<Vector2>();
        // convert Vector2 inputs into a Vector3 direction
        direction.x = vectorWASD.x;
        direction.z = vectorWASD.y;

        // apply force
        rigidBodyRef.AddForce(direction * force * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    // check if player is on the ground via raycast
    private void OnGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f))
        {
            onGround = true;
            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.red);
        }
        else
        {
            onGround = false;
            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.red);
        }
    }

    // if the player is not on the ground or jumping apply force down
    private void Gravity()
    {
        if (!jumping && !onGround)
        {
            rigidBodyRef.AddForce(transform.up * jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }

    // if player presses space apply force up
    private void Jump()
    {
        // get input values
     //   Vector2 boost = playerInputActions.PlayerActions.Boost.ReadValue<Vector2>();

        if (jumping)
        {
            rigidBodyRef.AddForce(transform.up * jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }

    // checks if player has fallen off the floor and teleports them back to origin, resetting force
    private void Deathfloor()
    {
        if (transform.position.y <= -15)
        {
            transform.position = new Vector3(0, 0, 0);
            // reset momentum
        }
    }
}