using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Main author: Axel Ingelsson Fredler

public class AxelPlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction mouseLookAction;
    private InputAction gamePadLookAction;
    private InputAction useAction;
    private InputAction saveAction;
    private InputAction LoadAction;
    private InputAction drillAction;
    private InputAction shootAction;
    [SerializeField] private GameObject drill;
    private Camera mainCamera;
    private EgilSaveAndLoadImplementation saveAndLoad;  
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] [Range(1.0f, 50.0f)] private float movementAcceleration = 5.0f;
    private Vector3 velocity;
    private Vector3 playerMovementInput;
    private Vector2 lookRotation;
    private String KeyboardAndMouseControlScheme = "Keyboard&Mouse";
    private String GamepadControlScheme = "Gamepad";
    private bool movementEnabled = true;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        mouseLookAction = playerInput.actions["MouseLook"];
        gamePadLookAction = playerInput.actions["GamePadLook"];
        useAction = playerInput.actions["Use"];
        saveAction = playerInput.actions["Save"];
        LoadAction = playerInput.actions["Load"];
        shootAction = playerInput.actions["Shoot"];
        drillAction = playerInput.actions["Drill"];
        saveAndLoad = GetComponent<EgilSaveAndLoadImplementation>();
        mainCamera = Camera.main;
    }
    
    private void Update()
    {
        PlayerMovement();
        ShootOrDrill();
        SaveAndLoadGame();
    }

    private void ShootOrDrill()
    {
        if (shootAction.IsPressed())
        {
            drill.gameObject.SendMessage("Shoot");
            drill.gameObject.SendMessage("DrillInUse", true);
        }
        else
        {
            if (drillAction.IsPressed())
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

    private void SaveAndLoadGame()
    {
        if (saveAction.WasPressedThisFrame())
        {
            saveAndLoad.saveGamePress();
        }

        if (LoadAction.WasPressedThisFrame())
        {
            saveAndLoad.LoadGamePress();
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
        // if (playerInput.currentControlScheme.Equals(KeyboardAndMouseControlScheme))
        // {
        //     UpdatePlayerPositionAndRotationKeyBoardAndMouse();
        // }
        if (playerInput.currentControlScheme.Equals(GamepadControlScheme))
        {
            UpdatePlayerPositionAndRotationGamePad();
        }
    }

    public bool IsUseInputPressed()
    {
        return useAction.WasPressedThisFrame();
    }

    public void SetMovementStatus(bool movementStatus)
    {
        movementEnabled = movementStatus;
    }

    private void UpdatePlayerPositionAndRotationGamePad()
    {
        UpdatePlayerRotationGamePad();
        UpdatePlayerPositionGamePad();
    }

    private void UpdatePlayerPositionGamePad()
    {
        playerMovementInput = moveAction.ReadValue<Vector2>();
        Vector3 gamePadMovement = new Vector3(playerMovementInput.x, 0.0f, playerMovementInput.y);
        gamePadMovement = transform.localRotation * gamePadMovement;
        Vector3 cv = mainCamera.WorldToViewportPoint(transform.position);
        cv.x = Mathf.Clamp01(cv.x);
        cv.y = Mathf.Clamp01(cv.y);
        velocity = (Vector3) moveAction.ReadValue<Vector2>() * movementAcceleration;
        transform.localPosition += gamePadMovement * movementAcceleration * Time.deltaTime;
    }

    private void UpdatePlayerRotationGamePad()
    {
        Vector3 gamePadLookRotation = gamePadLookAction.ReadValue<Vector2>();
        transform.forward += new Vector3(gamePadLookRotation.x, 0.0f, gamePadLookRotation.y);
    }

    private void UpdatePlayerPositionAndRotationKeyBoardAndMouse()
    {
        playerMovementInput = moveAction.ReadValue<Vector3>();
        velocity = new Vector3(playerMovementInput.x, 0.0f, playerMovementInput.z) * movementAcceleration * Time.deltaTime;
        PlayerMouseAim();
        velocity = transform.localRotation * velocity;
        transform.localPosition += velocity;
    }

    private void PlayerMouseAim()
    {
        Vector3 mousePosition = GetMousePosition();
        Vector3 mouseDirection = mousePosition - transform.localPosition;
        mouseDirection.y = 0;
        transform.forward = mouseDirection;
    }

    private Vector3 GetMousePosition()
    {
        Ray mouseRay = mainCamera.ScreenPointToRay(mouseLookAction.ReadValue<Vector2>());
        Physics.Raycast(mouseRay, out var hitInfo, Mathf.Infinity, groundLayerMask);
        return hitInfo.point;
    }
}
