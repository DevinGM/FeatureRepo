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

    public float speed = 10f;

    // Awake is called before the first frame update
    void Awake()
    {
        // get inputs
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Deathfloor();

        // movement
        Vector2 vectorWASD = playerInputActions.PlayerActions.MoveWASD.ReadValue<Vector2>();
        transform.Translate(vectorWASD * Time.deltaTime * speed);
    }

    // handles physics controlled movement
    private void FixedUpdate()
    {
        
    }

    private void Deathfloor()
    {
        if (transform.position.y <= -15)
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }
}