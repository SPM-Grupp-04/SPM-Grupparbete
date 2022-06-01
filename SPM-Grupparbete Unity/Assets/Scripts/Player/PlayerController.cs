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
    
    [SerializeField] private PlayerController otherPlayerController;
    [SerializeField] private GameObject teleport;
    
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
    
    private static bool  movementEnabled = true;

    private bool enteredShopArea;
    private bool isShooting;
    private bool isDrilling;
    private bool useButtonPressed;
    
    private static bool uiEnabled;
    private bool playerCanShop;

    private bool insideShield;

    private GameObject pauseMenuUI;
    private bool pauseButtonPressed;

    private InputActionMap UI;
    private InputActionMap defaultMap;

    private AudioSource source;

    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        mainCamera = Camera.main;
        source = GetComponent<AudioSource>();
        source.loop = true;
        playerInput = GetComponent<PlayerInput>();
        mainCamera = Camera.main;
        UI = playerInput.actions.FindActionMap("UI");
        defaultMap = playerInput.actions.FindActionMap("Player");
        pauseMenuUI = GameObject.Find("UI");
    }

    private void Update()
    {
        if (!UI_PausMenu.GameIsPause)
        {
            PlayerMovement();
        }
        if (!TownPortal.IsTeleporting)
        {
            RestrictMovement();
        }
        if (!uiEnabled && playerInput.currentActionMap.name.Equals("UI"))
        {
            playerInput.SwitchCurrentActionMap("Player");
        }
    }

    public void SwitchActionMap(InputAction.CallbackContext SwitchMap)
    {
        if(playerCanShop)
        {
            if (SwitchMap.performed)
            {
                uiEnabled = !uiEnabled;

                if (uiEnabled)
                {
                    playerInput.SwitchCurrentActionMap("UI");
                }
                else
                {
                    playerInput.SwitchCurrentActionMap("Player");
                }
            }
        }
    }

    public void PauseButtonInput(InputAction.CallbackContext pauseButtonValue)
    {
        if (pauseButtonValue.performed)
        {
            pauseButtonPressed = !pauseButtonPressed;
        }

        if (pauseButtonPressed)
        {
            pauseMenuUI.GetComponent<UI_PausMenu>().Pause();
        }
    }

    public Vector3 PlayerVelocity
    {
        get { return velocity; }
        set { velocity = value; }
    }

    public bool IsShooting
    {
        get { return isShooting; }
    }

    public bool IsDrilling
    {
        get { return isDrilling; }
    }

    public bool IsUseButtonPressed()
    {
        return useButtonPressed;
    }
    
    public bool IsShopOpen()
    {
        return uiEnabled;
    }
    
    public bool InsideShield
    {
        get { return insideShield; }
        set { insideShield = value; }
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

    public void PlayerCanShop(bool value)
    {
        playerCanShop = value;
    }

    public bool IsPauseMenuOpen()
    {
        return pauseButtonPressed;
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
    public void Teleport(InputAction.CallbackContext teleportValue)
    {
        if (teleport.activeInHierarchy) return;
        
        teleport.transform.position = transform.position + new Vector3(1, 1, 1);
        teleport.SetActive(true);
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
        Vector3 mousePositionOnScreen = GetMousePosition();
        Vector3 mouseDirection = (mousePositionOnScreen - transform.localPosition) * rotationSmoothing;
        mouseDirection.y = 0;
        transform.forward = mouseDirection;
    }

    private Vector3 GetMousePosition()
    {
        Ray mouseRay = mainCamera.ScreenPointToRay(mousePosition);
        Physics.Raycast(mouseRay, out var hitInfo, Mathf.Infinity, groundLayerMask);
        return hitInfo.point;
    }
    
    private void RestrictMovement()
    {
        Vector3 currentPlayerCameraView = mainCamera.WorldToViewportPoint(transform.position);
        currentPlayerCameraView.x = Mathf.Clamp01(currentPlayerCameraView.x);
        currentPlayerCameraView.y = Mathf.Clamp01(currentPlayerCameraView.y);

        Vector3 otherPlayerVelocity = otherPlayerController.PlayerVelocity;
        
        if (currentPlayerCameraView.x <= 0.05 || currentPlayerCameraView.x >= 0.95f)
        {
            if (otherPlayerVelocity.x > 0.0f)
            {
                otherPlayerController.PlayerVelocity = new Vector3(-otherPlayerVelocity.x, otherPlayerVelocity.y, otherPlayerVelocity.z);
            }

            if (otherPlayerVelocity.x < 0.0f)
            {
                otherPlayerController.PlayerVelocity = new Vector3(+otherPlayerVelocity.x, otherPlayerVelocity.y, otherPlayerVelocity.z);
            }
        }
        if (currentPlayerCameraView.y <= 0.05f || currentPlayerCameraView.y >= 0.95f)
        {
            if (otherPlayerVelocity.z > 0.0f)
            {
                otherPlayerController.PlayerVelocity = new Vector3(otherPlayerVelocity.x, otherPlayerVelocity.y,
                    -otherPlayerVelocity.z);
            }
            if (otherPlayerVelocity.z < 0.0f)
            {
                otherPlayerController.PlayerVelocity = new Vector3(otherPlayerVelocity.x, otherPlayerVelocity.y, +otherPlayerVelocity.z);
            }
        }
        
        Vector3 currentPlayerPosInWorldPoint = mainCamera.ViewportToWorldPoint(currentPlayerCameraView);
        
        transform.position = new Vector3(currentPlayerPosInWorldPoint.x, transform.position.y, currentPlayerPosInWorldPoint.z);
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

    private void PlayerMovement()
    {
        if (movementEnabled)
        {
            UpdatePlayer();
            FixOverlapPenetration();
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
    
    private void OnEnable()
    {
        playerInput.actions["SwitchMap"].performed += SwitchActionMap;
    }

    private void OnDisable()
    {
        playerInput.actions["SwitchMap"].performed -= SwitchActionMap;
    }
  
}