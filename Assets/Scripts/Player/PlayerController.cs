using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private float movementSpeed = 1.0f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private bool interacted;
    private Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        playerControls.Interaction.Interact.performed += OnInteract;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();

    }

    private void FixedUpdate()
    {
        Move();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * (movementSpeed * Time.fixedDeltaTime));
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Player Interacted");
        // Add your interaction logic here
    }
}
