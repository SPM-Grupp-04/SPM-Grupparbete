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
    private PlayerInput _PlayerInput;
    private InputAction _MoveAction;
    private InputAction _MouseLookAction;
    private InputAction _GamePadLookAction;
    private InputAction _FireAction;
    private Camera _MainCamera;
    [SerializeField] private LayerMask _GroundLayerMask;
    private Vector3 _Velocity;
    private Vector3 _PlayerMovementInput;
    private Vector2 _LookRotation;
    private String _KEYBOARDANDMOUSECONTROLSCHEME = "Keyboard&Mouse";
    private String _GAMEPADCONTROLSCHEME = "Gamepad";
    [FormerlySerializedAs("_Acceleration")] [SerializeField] [Range(1.0f, 50.0f)] private float _MovementAcceleration = 5.0f;

    private void Awake()
    {
        _PlayerInput = GetComponent<PlayerInput>();
        _MoveAction = _PlayerInput.actions["Move"];
        _MouseLookAction = _PlayerInput.actions["MouseLook"];
        _GamePadLookAction = _PlayerInput.actions["GamePadLook"];
        _FireAction = _PlayerInput.actions["Fire"];
        _MainCamera = Camera.main;
    }
    
    private void Update()
    {   
        if (_PlayerInput.currentControlScheme.Equals(_KEYBOARDANDMOUSECONTROLSCHEME))
        {
            UpdatePlayerPositionAndRotationKeyBoardAndMouse();
        } else if (_PlayerInput.currentControlScheme.Equals(_GAMEPADCONTROLSCHEME))
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
        _PlayerMovementInput = _MoveAction.ReadValue<Vector2>();
        Vector3 _gamePadMovement = new Vector3(_PlayerMovementInput.x, 0.0f, _PlayerMovementInput.y);
        _gamePadMovement = transform.localRotation * _gamePadMovement;
        transform.position += _gamePadMovement * _MovementAcceleration * Time.deltaTime;
    }

    private void UpdatePlayerRotationGamePad()
    {
        Vector3 _gamePadLookRotation = _GamePadLookAction.ReadValue<Vector2>();
        transform.forward += new Vector3(_gamePadLookRotation.x, 0.0f, _gamePadLookRotation.y);
    }

    private void UpdatePlayerPositionAndRotationKeyBoardAndMouse()
    {
        _PlayerMovementInput = _MoveAction.ReadValue<Vector3>();
        _Velocity = new Vector3(_PlayerMovementInput.x, 0.0f, _PlayerMovementInput.z) * _MovementAcceleration * Time.deltaTime;
        PlayerMouseAim();
        _Velocity = transform.localRotation * _Velocity;
        transform.position += _Velocity;
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
        Ray mouseRay = _MainCamera.ScreenPointToRay(_MouseLookAction.ReadValue<Vector2>());
        Physics.Raycast(mouseRay, out var hitInfo, Mathf.Infinity, _GroundLayerMask);
        if (Physics.Raycast(mouseRay, out var hit, 100))
        {
            if (_FireAction.WasPerformedThisFrame() && hit.transform.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("BANG!");
            }
        }
        return hitInfo.point;
    }
}
