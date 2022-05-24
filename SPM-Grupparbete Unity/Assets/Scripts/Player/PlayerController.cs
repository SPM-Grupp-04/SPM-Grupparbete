using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
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
    [SerializeField] private AudioClip drillSound, laserSound;
    [SerializeField] private Animator animator;
    
    private PlayerDrill drillScript;
    
    [Header("Player Restrictions")] 
    [SerializeField] [Range(0.9f, 1.0f)] private float cameraPlayerPositiveMovementThreshold = 0.9f;
    [SerializeField] [Range(0.01f, 0.1f)] private float cameraPlayerNegativeMovementThreshold = 0.1f;
    
    [SerializeField] private PlayerController otherPlayerController;
    
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

    private bool uiEnabled;

    private InputActionMap UI;
    private InputActionMap defaultMap;

    private AudioSource source;


    private void Awake()
    {
       // animator.enabled = true;
        source = GetComponent<AudioSource>();
        source.loop = true;
        playerInput = GetComponent<PlayerInput>();
        mainCamera = Camera.main;
        drillScript = drill.GetComponent<PlayerDrill>();
        UI = playerInput.actions.FindActionMap("UI");
        defaultMap = playerInput.actions.FindActionMap("Player");
    }

    private void Update()
    {
        if (UI_PausMenu.GameIsPause == false)
        {
            PlayerMovement();
            ShootOrDrill();
        }

        if (TownPortal.isTeleporting == false)
        {
            RestrictMovement();
        }
    }

    private void OnEnable()
    {
        playerInput.actions["SwitchMap"].performed += SwitchActionMap;
    }

    private void OnDisable()
    {
        playerInput.actions["SwitchMap"].performed -= SwitchActionMap;
    }

    public void SwitchActionMap(InputAction.CallbackContext SwitchMap)
    {
        if (SwitchMap.performed)
        {
            uiEnabled = !uiEnabled;

            if (uiEnabled)
            {
                UI.Enable();
                playerInput.SwitchCurrentActionMap("UI");
                
                defaultMap.Disable();

                Debug.Log(uiEnabled + playerInput.currentActionMap.ToString());
            }
            else
            {
                //Debug.Log(uiEnabled);

                //defaultMap.Enable();
                playerInput.SwitchCurrentActionMap("Player");
                UI.Disable();

                Debug.Log(uiEnabled + playerInput.currentActionMap.ToString());

            }
        }
    }
    
    private void RestrictMovement()
    {
        Vector3 currentPlayerCameraView = mainCamera.WorldToViewportPoint(transform.position);
        currentPlayerCameraView.x = Mathf.Clamp01(currentPlayerCameraView.x);
        currentPlayerCameraView.y = Mathf.Clamp01(currentPlayerCameraView.y);

        Vector3 otherPlayerVelocity = otherPlayerController.GetPlayerVelocity();
        
        if (currentPlayerCameraView.x <= 0.05 || currentPlayerCameraView.x >= 0.95f)
        {
            if (otherPlayerVelocity.x > 0.0f)
            {
                otherPlayerController.SetPlayerVelocity(new Vector3(-otherPlayerVelocity.x, otherPlayerVelocity.y, otherPlayerVelocity.z));
            }

            if (otherPlayerVelocity.x < 0.0f)
            {
                otherPlayerController.SetPlayerVelocity(new Vector3(+otherPlayerVelocity.x, otherPlayerVelocity.y, otherPlayerVelocity.z));
            }
        }
        if (currentPlayerCameraView.y <= 0.05f || currentPlayerCameraView.y >= 0.95f)
        {
            if (otherPlayerVelocity.z > 0.0f)
            {
                otherPlayerController.SetPlayerVelocity(new Vector3(otherPlayerVelocity.x, otherPlayerVelocity.y, -otherPlayerVelocity.z));
            }
            if (otherPlayerVelocity.z < 0.0f)
            {
                otherPlayerController.SetPlayerVelocity(new Vector3(otherPlayerVelocity.x, otherPlayerVelocity.y, +otherPlayerVelocity.z));
            }
        }
        
        Vector3 currentPlayerPosInWorldPoint = mainCamera.ViewportToWorldPoint(currentPlayerCameraView);
        
        transform.position = new Vector3(currentPlayerPosInWorldPoint.x, transform.position.y, currentPlayerPosInWorldPoint.z);
    }

    private void ShootOrDrill()
    {
        if (isShooting)
        {


            drillScript.Shoot(true);
            drillScript.DrillInUse(true);
            drillScript.Drill(false);
            animator.SetBool("IsShooting", true);
            animator.SetBool("Idle", false);
            Debug.Log(animator.isActiveAndEnabled);
            

            if (!source.isPlaying)
            {
                PlayLaserWeaponSound();
            }
        }
        else
        {
            if (isDrilling)
            {
                drillScript.Shoot(false);
                drillScript.Drill(true);
                drillScript.DrillInUse(true);

                animator.SetBool("IsShooting", true);
                animator.SetBool("Idle", false);

                if (!source.isPlaying)
                {
                    PlayDrillSound();
                }
            }
            else
            {
                drillScript.DrillInUse(false);
                drillScript.Shoot(false);
                drillScript.Drill(false);
                StopSound();

                animator.SetBool("IsShooting", false);
                animator.SetBool("Idle", true);

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

    public bool IsMapSwitched()
    {
        return uiEnabled;
    }

    public void ShootInput(InputAction.CallbackContext shootValue)
    {
        if (shootValue.performed)
        {
            isShooting = true;
        }
        else if (shootValue.canceled)
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

    private void PlayLaserWeaponSound()
    {
        source.clip = laserSound;
        source.Play();
    }

    private void PlayDrillSound()
    {
        source.clip = drillSound;
        source.Play();
    }

    private void StopSound()
    {
        source.Stop();
        source.clip = null;
    }

    public void UseInput(InputAction.CallbackContext useValue)
    {
        //useButtonPressed = useValue.performed;

        if (useValue.performed)
        {
            useButtonPressed = true;
        }
        else if (useValue.canceled)
        {
            useButtonPressed = false;
        }
    }

    [SerializeField] private GameObject teleport;

    public void Teleport(InputAction.CallbackContext teleportValue)
    {
        if (teleport.activeInHierarchy != false) return;

        teleport.transform.position = transform.position + new Vector3(1, 1, 1);
        teleport.SetActive(true);
    }

    public void SetMovementStatus(bool movementStatus)
    {
        movementEnabled = movementStatus;
    }

    public void PlayerMovementInput(InputAction.CallbackContext moveValue)
    {
        playerMovementInput = moveValue.ReadValue<Vector2>();
        velocity = new Vector3(playerMovementInput.x, 0.0f, playerMovementInput.y);
        if (velocity != Vector3.zero)
        {
            //animator.SetBool("MoveForward", true);
        }
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