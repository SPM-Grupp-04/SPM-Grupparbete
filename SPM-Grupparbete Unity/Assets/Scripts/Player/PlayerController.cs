using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

//Main author: Axel Ingelsson Fredler

public class PlayerController : MonoBehaviour
{
    [Header("Collision Layer Masks")]
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask wallLayerMask;
    
    [Header("Movement")]
    [SerializeField] [Range(1.0f, 50.0f)] private float movementAcceleration = 5.0f;
    [SerializeField] [Range(1.0f, 1000f)] private float rotationSmoothing = 1000.0f;
    [SerializeField] [Range(0.0f, 1.0f)] private float colliderMargin = 0.01f;
    
    [Header("Components")]
    [SerializeField] private GameObject drill;
    [SerializeField] private BoxCollider boxCollider;
    
    [SerializeField] private Transform otherPlayerTransform;

    private RestrictPlayerMovementOutsideCamera restrictPlayerMovementOutSideCamera;
    
    private Collider[] penetrationColliders = new Collider[2];
    
    private PlayerInput playerInput;
    
    private Camera mainCamera;
    
    private Vector3 velocity;
    private Vector3 playerMovementInput;
    private Vector3 gamePadLookRotation;
    private Vector2 lookRotation;
    private Vector2 mousePosition;


    private bool invisibleWallDeployed;
    private Vector3 invisibleWallPosition;
    
    private String KeyboardAndMouseControlScheme = "Keyboard&Mouse";
    private String GamepadControlScheme = "Gamepad";
    
    private bool movementEnabled = true;
    private bool enteredShopArea;
    private bool isShooting;
    private bool isDrilling;
    private bool useButtonPressed;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        mainCamera = Camera.main;
        restrictPlayerMovementOutSideCamera = RestrictPlayerMovementOutsideCamera.Instance;
    }

    private void Update()
    {
        if (PausMenu.GameIsPause == false)
        {
            PlayerMovement();
            ShootOrDrill();
            FixOverlapPenetration();
        }
        RestrictMovement();
    }

    private void RestrictMovement()
    {
        Vector3 currentPlayerCameraView = mainCamera.WorldToViewportPoint(transform.position);
        currentPlayerCameraView.x = Mathf.Clamp01(currentPlayerCameraView.x);
        currentPlayerCameraView.y = Mathf.Clamp01(currentPlayerCameraView.y);

        if (currentPlayerCameraView.x == 0.0f && !invisibleWallDeployed)
        {
            invisibleWallDeployed = true;
            restrictPlayerMovementOutSideCamera.MoveInvisibleWallToBlockPlayer(new Vector3(otherPlayerTransform.position.x + 1.0f, 10.0f, 0.0f), quaternion.Euler(0.0f, 90.0f, 0.0f));
        } else if (currentPlayerCameraView.x > 0.1f && invisibleWallDeployed)
        {
            invisibleWallDeployed = false;
            restrictPlayerMovementOutSideCamera.ResetInvisibleWall();
        }

        if (currentPlayerCameraView.x == 1.0f && !invisibleWallDeployed)
        {
            invisibleWallDeployed = true;
            restrictPlayerMovementOutSideCamera.MoveInvisibleWallToBlockPlayer(new Vector3(otherPlayerTransform.position.x - 1.0f, 10.0f, 0.0f), quaternion.Euler(0.0f, 90.0f, 0.0f));
        } else if (currentPlayerCameraView.x < 0.9f && invisibleWallDeployed)
        {
            invisibleWallDeployed = false;
            restrictPlayerMovementOutSideCamera.ResetInvisibleWall();
        }

        if (currentPlayerCameraView.y == 0.0f && !invisibleWallDeployed)
        {
            invisibleWallDeployed = true;
            restrictPlayerMovementOutSideCamera.MoveInvisibleWallToBlockPlayer(new Vector3(0.0f, 10.0f, otherPlayerTransform.position.z + 1.0f), quaternion.Euler(0.0f, 0.0f, 0.0f));
        } else if (currentPlayerCameraView.y > 0.1f && invisibleWallDeployed)
        {
            invisibleWallDeployed = false;
            restrictPlayerMovementOutSideCamera.ResetInvisibleWall();
        }

        if (currentPlayerCameraView.y == 1.0f && !invisibleWallDeployed)
        {
            invisibleWallDeployed = true;
            restrictPlayerMovementOutSideCamera.MoveInvisibleWallToBlockPlayer(new Vector3(0.0f, 10.0f, otherPlayerTransform.position.z - 1.0f), quaternion.Euler(0.0f, 0.0f, 0.0f));
        } else if (currentPlayerCameraView.y < 0.9f && invisibleWallDeployed)
        {
            invisibleWallDeployed = false;
            restrictPlayerMovementOutSideCamera.ResetInvisibleWall();
        }

        Vector3 currentPlayerPosInWorldPoint = mainCamera.ViewportToWorldPoint(currentPlayerCameraView);
        
        transform.position = new Vector3(currentPlayerPosInWorldPoint.x, transform.position.y, currentPlayerPosInWorldPoint.z);
    }

    private void ShootOrDrill()
    {
        if (isShooting)
        {
            Debug.Log("SHOOT");
            drill.gameObject.SendMessage("Shoot", true);
            drill.gameObject.SendMessage("DrillInUse", true);
        }
        else
        {
            if (isDrilling)
            {
                drill.gameObject.SendMessage("DrillObject");
                drill.gameObject.SendMessage("DrillInUse", true);
            }
            else
            {
                drill.gameObject.SendMessage("DrillInUse", false);
            }
        }
    }

    private void PlayerMovement()
    {
        if (movementEnabled)
        {
            UpdatePlayer();
        }
        else
        {
            velocity = Vector3.zero;
        }
    }

    private void UpdatePlayer()
    {
        if (playerInput.currentControlScheme.Equals(KeyboardAndMouseControlScheme))
        {
            UpdatePlayerRotationKeyBoardAndMouse();
        }

        if (playerInput.currentControlScheme.Equals(GamepadControlScheme))
        {
            UpdatePlayerRotationGamePad();
        }
        transform.position += velocity * movementAcceleration * Time.deltaTime;
    }

    public void SetPlayerVelocity(Vector3 newPlayerVelocity)
    {
        velocity = newPlayerVelocity;
    }

    public Vector3 GetPlayerVelocity()
    {
        return velocity;
    }

    public bool IsUseButtonPressed()
    {
        return useButtonPressed;
    }
    
    public void ShootInput(InputAction.CallbackContext shootValue)
    {
        if (shootValue.performed)
        {
            isShooting = true;
        } else if (shootValue.canceled)
        {
            isShooting = false;
        }
    }

    public void DrillInput(InputAction.CallbackContext drillValue)
    {
        if (drillValue.performed)
        {
            isDrilling = true;
        }
        else if (drillValue.canceled)
        {
            isDrilling = false;
        }
    }
    
    public void UseInput(InputAction.CallbackContext useValue)
    {
        useButtonPressed = useValue.performed;
    }

    public void SetMovementStatus(bool movementStatus)
    {
        movementEnabled = movementStatus;
    }
    
    public void PlayerMovementInput(InputAction.CallbackContext moveValue)
    {
        playerMovementInput = moveValue.ReadValue<Vector2>();
        velocity = new Vector3(playerMovementInput.x, 0.0f, playerMovementInput.y);
    }

    public void PlayerRotationGamePadInput(InputAction.CallbackContext rotationValue)
    {
        gamePadLookRotation = rotationValue.ReadValue<Vector2>();
    }
    
    public void PlayerMousePositionInput(InputAction.CallbackContext mouseValue)
    {
        mousePosition = mouseValue.ReadValue<Vector2>();
    }

    private void UpdatePlayerRotationGamePad()
    {
        transform.forward += new Vector3(gamePadLookRotation.x, 0.0f, gamePadLookRotation.y) * rotationSmoothing *
                             Time.deltaTime;
    }

    private void UpdatePlayerRotationKeyBoardAndMouse()
    {
        PlayerMouseAim();
    }

    private void PlayerMouseAim()
    {
        Vector3 mousePosition = GetMousePosition();
        Vector3 mouseDirection = (mousePosition - transform.localPosition) * rotationSmoothing;
        mouseDirection.y = 0;
        transform.forward = mouseDirection;
    }

    private Vector3 GetMousePosition()
    {
        Ray mouseRay = mainCamera.ScreenPointToRay(mousePosition);
        Physics.Raycast(mouseRay, out var hitInfo, Mathf.Infinity, groundLayerMask);
        return hitInfo.point;
    }
    
    private void FixOverlapPenetration()
    {
        int colliderCount = Physics.OverlapBoxNonAlloc(transform.position, boxCollider.size / 2, penetrationColliders,
            boxCollider.transform.rotation, wallLayerMask);

        while (colliderCount > 0)
        {
            for (int i = 0; i < colliderCount; i++)
            {
                if (Physics.ComputePenetration(boxCollider, boxCollider.transform.position, boxCollider.transform.rotation,
                        penetrationColliders[i], penetrationColliders[i].gameObject.transform.position, penetrationColliders[i].gameObject.transform.rotation,
                        out var direction, out var distance))
                {
                    Vector3 separationVector = direction * distance;
                    transform.position += separationVector + separationVector.normalized * colliderMargin;
                }
            }
            colliderCount = Physics.OverlapBoxNonAlloc(transform.position, boxCollider.size / 2, penetrationColliders,
                boxCollider.transform.rotation, wallLayerMask);
        }
    }
}