using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    #region Enums
    private enum Directions { UP, DOWN, LEFT, RIGHT }

    #endregion

    #region Editor Data

    [Header("Movement Attributes")]
    [SerializeField] private float movementSpeed = 1.0f;
    public bool AllowMovement { get; set; } = true;

    [Header("Dependencies")]
    private PlayerControls playerControls;
    private Vector2 movement;
    private bool interacted;
    private Rigidbody2D rb;
    private Collider2D currentInteractable;
    public Animator animator;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private LayerMask interactableLayer;


    #endregion

    #region Internal Data
    [SerializeField] public float interactionCooldown = 0.2f;
    private float lastInteractionTime;
    
    private Directions _facingDirection = Directions.UP;
    private readonly int _animMoveRight = Animator.StringToHash("Anim_Player_Move_Right");
    private readonly int _animIdleFront = Animator.StringToHash("Anim_Player_Idle_Front");
    private readonly int _animMoveBack = Animator.StringToHash("Anim_Player_Move_Up");
    private readonly int _animMoveDown = Animator.StringToHash("Anim_Player_Move_Down");


    #endregion


    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerControls.Interaction.Interact.performed += OnInteract;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    #region Tick
    public void HandleUpdate()
    {
        if (!AllowMovement) return;

        PlayerInput();
        CalculateFacingDirection();
        
        UpdateAnimation();

    }

    private void FixedUpdate()
    {
        if (!AllowMovement) return;

        Move();
    }

    #endregion


    #region Input Logic
    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

    }

    #endregion

    #region Movement Logic
    private void Move()
    {
        rb.MovePosition(rb.position  + movement * (movementSpeed * Time.fixedDeltaTime));
    }

    #endregion

    #region Animation Logic

    private void CalculateFacingDirection()
    {
        if (movement.x != 0)
        {
            if (movement.x > 0) // Moving Right
            {
                _facingDirection = Directions.RIGHT;
            }
            else if (movement.x < 0) // Moving Left
            {
                _facingDirection = Directions.LEFT;
            }
        }
        else if (movement.y != 0)
        {
            if (movement.y > 0) // Moving Up
            {
                _facingDirection = Directions.UP;
            }
            else if (movement.y < 0) // Moving Down
            {
                _facingDirection = Directions.DOWN;
            }
        }

        Debug.Log(_facingDirection);
    }

    private void UpdateAnimation()
    {
        // Sprite flipping for left/right
        if (_facingDirection == Directions.LEFT)
        {
            spriteRenderer.flipX = true;
        }
        else if (_facingDirection == Directions.RIGHT)
        {
            spriteRenderer.flipX = false;
        }

        // Animation selection based on movement and facing direction
        if (movement != Vector2.zero)
        {
            switch (_facingDirection)
            {
                case Directions.UP:
                    animator.CrossFade(_animMoveBack, 0);
                    break;
                case Directions.RIGHT:
                    animator.CrossFade(_animMoveRight, 0);
                    break;
                case Directions.LEFT:
                    animator.CrossFade(_animMoveRight, 0);
                    break;
                case Directions.DOWN:
                    animator.CrossFade(_animMoveDown, 0);
                    break;
                default:
                    animator.CrossFade(_animIdleFront, 0);
                    break;
            }
        }
        else
        {
            animator.CrossFade(_animIdleFront, 0);
        }
    }

    #endregion



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
