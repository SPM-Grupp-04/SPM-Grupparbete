using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Main author: Axel Ingelsson Fredler

public class PlayerController : MonoBehaviour
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
    private SaveAndLoadSystem saveAndLoad;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] [Range(1.0f, 50.0f)] private float movementAcceleration = 5.0f;
    [SerializeField] [Range(1.0f, 10f)] private float rotationSmoothing = 5.0f;
    private Vector3 velocity;
    private Vector3 playerMovementInput;
    private Vector2 lookRotation;
    private String KeyboardAndMouseControlScheme = "Keyboard&Mouse";
    private String GamepadControlScheme = "Gamepad";
    private bool movementEnabled = true;
    private bool enteredShopArea;

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
        saveAndLoad = GetComponent<SaveAndLoadSystem>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (PausMenu.GameIsPause == false)
        {
            PlayerMovement();
            ShootOrDrill();
        }

        
        RestrictMovement();
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
            UpdatePlayerPositionAndRotationKeyBoardAndMouse();
        }

        if (playerInput.currentControlScheme.Equals(GamepadControlScheme))
        {
            UpdatePlayerPositionAndRotationGamePad();
        }
    }

    public bool hasEnteredShopArea()
    {
        return enteredShopArea;
    }

    public void setEnteredShopArea(bool enteredShopAreaState)
    {
        enteredShopArea = enteredShopAreaState;
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
        velocity = new Vector3(playerMovementInput.x, 0.0f, playerMovementInput.y) * movementAcceleration;
        transform.localPosition += velocity * Time.deltaTime;
    }

    private void UpdatePlayerRotationGamePad()
    {
        Vector3 gamePadLookRotation = gamePadLookAction.ReadValue<Vector2>();
        transform.forward += new Vector3(gamePadLookRotation.x, 0.0f, gamePadLookRotation.y) * rotationSmoothing *
                             Time.deltaTime;
    }

    private void UpdatePlayerPositionAndRotationKeyBoardAndMouse()
    {
        playerMovementInput = moveAction.ReadValue<Vector3>();
        velocity = new Vector3(playerMovementInput.x, 0.0f, playerMovementInput.z) * movementAcceleration *
                   Time.deltaTime;
        PlayerMouseAim();
        transform.localPosition += velocity;
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
        Ray mouseRay = mainCamera.ScreenPointToRay(mouseLookAction.ReadValue<Vector2>());
        Physics.Raycast(mouseRay, out var hitInfo, Mathf.Infinity, groundLayerMask);
        return hitInfo.point;
    }
}