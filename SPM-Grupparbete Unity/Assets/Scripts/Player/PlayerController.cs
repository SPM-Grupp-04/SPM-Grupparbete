using System;
using System.Collections;
using System.Collections.Generic;
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
    
    private Collider[] penetrationColliders = new Collider[2];
    
    private PlayerInput playerInput;
    
    private Camera mainCamera;
    
    private Vector3 velocity;
    private Vector3 playerMovementInput;
    private Vector3 gamePadLookRotation;
    private Vector2 lookRotation;
    private Vector2 mousePosition;
    
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
        

        bool isOutSide = false;

        if (currentPlayerCameraView.x == 0f || currentPlayerCameraView.x == 1)
        {
            isOutSide = true;
            //  Debug.Log("Outside X ");
        }

        if (currentPlayerCameraView.y == 0f || currentPlayerCameraView.y == 1)
        {
            isOutSide = true;
            //  Debug.Log("Outside Y");
        }

        Vector3 currentPlayerPosInWorldPoint = mainCamera.ViewportToWorldPoint(currentPlayerCameraView);
        
        if (isOutSide)
        {
            Vector3 otherPlayerCameraView = mainCamera.WorldToViewportPoint(otherPlayerTransform.position);
            otherPlayerCameraView.x = Mathf.Clamp01(otherPlayerCameraView.x);
            otherPlayerCameraView.y = Mathf.Clamp01(otherPlayerCameraView.y);
            
            Vector3 otherPlayerPosInWorldPoint = mainCamera.ViewportToWorldPoint(otherPlayerCameraView);
            
            otherPlayerTransform.position = new Vector3(otherPlayerCameraView.x - otherPlayerPosInWorldPoint.x, otherPlayerTransform.position.y,
                otherPlayerCameraView.y - otherPlayerPosInWorldPoint.z);
            //    Debug.Log("PlayerPosInWorld " + playerPosInWorldPoint);
            Debug.Log("IS OUTSIDE");
        }
        
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