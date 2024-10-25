using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private float movementSpeed = 1.0f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private bool interacted;
    private Rigidbody2D rb;
    private Collider2D currentInteractable;


    [SerializeField] private LayerMask interactableLayer;

    [SerializeField] public float interactionCooldown = 0.2f;
    private float lastInteractionTime;

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

    public void HandleUpdate()
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object is in the interactable layer
        if ((interactableLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            currentInteractable = other;
            Debug.Log("Interactable Object Nearby");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Clear interactable if player leaves the collider
        if (other == currentInteractable)
        {
            currentInteractable = null;
            Debug.Log("Left Interactable Range");
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {

        // Check if enough time has passed since the last interaction
        if (Time.time - lastInteractionTime < interactionCooldown) return;

        if (GameController.Instance.state == GameState.FreeRoam)
        {
            Debug.Log("Player Interacted");

            if (currentInteractable != null)
            {
                Debug.Log("Player interacted with " + currentInteractable.name);

                // Trigger dialogue or interaction behavior
                var interactable = currentInteractable.GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }

            lastInteractionTime = Time.time;
        }
    }


}
