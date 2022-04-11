using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _PlayerInput;
    private InputAction _MoveAction;
    private InputAction _LookAction;
    private Camera _MainCamera;
    [SerializeField] private LayerMask _GroundLayerMask;
    private Vector3 _Velocity;
    private Vector3 _PlayerMovementInput;
    private Vector2 _LookRotation;
    [FormerlySerializedAs("_Acceleration")] [SerializeField] [Range(1.0f, 50.0f)] private float _MovementAcceleration = 5.0f;

    private void Awake()
    {
        _PlayerInput = GetComponent<PlayerInput>();
        _MoveAction = _PlayerInput.actions["Move"];
        _LookAction = _PlayerInput.actions["Look"];
        _MainCamera = Camera.main;
    }
    
    private void Update()
    {
        UpdatePlayerPositionAndRotation();
    }

    private void UpdatePlayerPositionAndRotation()
    {
        _PlayerMovementInput = _MoveAction.ReadValue<Vector3>();
        _Velocity = new Vector3(_PlayerMovementInput.x, 0.0f, _PlayerMovementInput.z) * _MovementAcceleration * Time.deltaTime;
        PlayerAim();
        _Velocity = transform.localRotation * _Velocity;
        transform.position += _Velocity;
    }

    private void PlayerAim()
    {
        Vector3 mousePosition = GetMousePosition();
        Vector3 mouseDirection = mousePosition - transform.position;
        mouseDirection.y = 0;
        transform.forward = mouseDirection;
    }

    private Vector3 GetMousePosition()
    {
        Ray mouseRay = _MainCamera.ScreenPointToRay(_LookAction.ReadValue<Vector2>());
        Physics.Raycast(mouseRay, out var hitInfo, Mathf.Infinity, _GroundLayerMask);
        return hitInfo.point;
    }
}
