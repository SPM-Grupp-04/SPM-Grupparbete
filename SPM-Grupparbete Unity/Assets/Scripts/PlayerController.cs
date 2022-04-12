using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

//Main author: Axel Ingelsson Fredler

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction mouseLookAction;
    private InputAction gamePadLookAction;
    private InputAction fireAction;
    private Camera mainCamera;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] [Range(1.0f, 50.0f)] private float movementAcceleration = 5.0f;
    private Vector3 velocity;
    private Vector3 playerMovementInput;
    private Vector2 lookRotation;
    private String KeyboardAndMouseControlScheme = "Keyboard&Mouse";
    private String GamepadControlScheme = "Gamepad";

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        mouseLookAction = playerInput.actions["MouseLook"];
        gamePadLookAction = playerInput.actions["GamePadLook"];
        fireAction = playerInput.actions["Fire"];
        mainCamera = Camera.main;
    }
    
    private void Update()
    {   
        if (playerInput.currentControlScheme.Equals(Keyboardandmousecontrolscheme))
        {
            UpdatePlayerPositionAndRotationKeyBoardAndMouse();
        } else if (playerInput.currentControlScheme.Equals(Gamepadcontrolscheme))
        {
            UpdatePlayerPositionAndRotationGamePad();
        }
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
        transform.position += gamePadMovement * movementAcceleration * Time.deltaTime;
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
        transform.position += velocity;
    }

    private void PlayerMouseAim()
    {
        Vector3 mousePosition = GetMousePosition();
        Vector3 mouseDirection = mousePosition - transform.position;
        Debug.DrawRay(transform.position, mouseDirection, Color.green);
        mouseDirection.y = 0;
        transform.forward = mouseDirection;
    }

    private Vector3 GetMousePosition()
    {
        Ray mouseRay = mainCamera.ScreenPointToRay(mouseLookAction.ReadValue<Vector2>());
        Physics.Raycast(mouseRay, out var hitInfo, Mathf.Infinity, groundLayerMask);
        if (Physics.Raycast(mouseRay, out var hit, 100))
        {
            if (fireAction.WasPerformedThisFrame() && hit.transform.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("BANG!");
            }
        }
        return hitInfo.point;
    }
}
