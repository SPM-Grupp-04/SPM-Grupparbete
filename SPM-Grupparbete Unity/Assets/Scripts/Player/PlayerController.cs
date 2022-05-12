using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Main author: Axel Ingelsson Fredler

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject drill;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] [Range(1.0f, 50.0f)] private float movementAcceleration = 5.0f;
    [SerializeField] [Range(1.0f, 1000f)] private float rotationSmoothing = 1000.0f;
    
    [SerializeField] private AudioClip drillSound, laserSound;

    
    private Animator animator;
    
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
        source = GetComponent<AudioSource>();
        source.loop = true;
        playerInput = GetComponent<PlayerInput>();
        mainCamera = Camera.main;

        UI = playerInput.actions.FindActionMap("UI");
        defaultMap = playerInput.actions.FindActionMap("Player");

        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        if (UI_PausMenu.GameIsPause == false)
        {
            PlayerMovement();
            ShootOrDrill();
        }
        RestrictMovement();
        
        
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
                Debug.Log(uiEnabled);

                defaultMap.Enable();
                playerInput.SwitchCurrentActionMap("Player");
                UI.Disable();
                Debug.Log(uiEnabled + playerInput.currentActionMap.ToString());

            }
        }
    }

    private void RestrictMovement()
    {
        Vector3 cameraView = mainCamera.WorldToViewportPoint(transform.position);
        cameraView.x = Mathf.Clamp01(cameraView.x);
        cameraView.y = Mathf.Clamp01(cameraView.y);

        bool isOutSide = false;

        if (cameraView.x == 0f || cameraView.x == 1)
        {
            isOutSide = true;
            //  Debug.Log("Outside X ");
        }

        if (cameraView.y == 0f || cameraView.y == 1)
        {
            isOutSide = true;
            //  Debug.Log("Outside Y");
        }

        Vector3 playerPosInWorldPoint = mainCamera.ViewportToWorldPoint(cameraView);
        if (isOutSide)
        {
            //    Debug.Log("PlayerPosInWorld " + playerPosInWorldPoint);
        }

        transform.position = new Vector3(playerPosInWorldPoint.x, transform.position.y, playerPosInWorldPoint.z);
    }

    private void ShootOrDrill()
    {
        if (isShooting)
        {
            Debug.Log("SHOOT");
            drill.gameObject.SendMessage("Shoot", true);
            drill.gameObject.SendMessage("DrillInUse", true);
            if (!source.isPlaying)
            {
                PlayLaserWeaponSound();
            }

        }
        else
        {
            if (isDrilling)
            {
                drill.gameObject.SendMessage("DrillObject");
                drill.gameObject.SendMessage("DrillInUse", true);
                if (!source.isPlaying)
                {
                    PlayDrillSound();
                }
            }
            else
            {
                drill.gameObject.SendMessage("DrillInUse", false);
                StopSound();
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
            
            animator.SetBool("Idel",true);
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

    public bool IsMapSwitched()
    {
        return uiEnabled;
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
            useButtonPressed = !useButtonPressed;
        } 
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
}