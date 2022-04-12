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
    private Camera mainCamera;
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
        mainCamera = Camera.main;
    }
    
    private void Update()
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
        } else if (playerInput.currentControlScheme.Equals(GamepadControlScheme))
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
