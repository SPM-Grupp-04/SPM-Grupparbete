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
    [SerializeField] private CinemachineVirtualCamera _PlayerFollowCamera;
    private Vector3 _Velocity;
    private Vector3 _PlayerMovementInput;
    private float _lookRotation;
    [FormerlySerializedAs("_Acceleration")] [SerializeField] [Range(1.0f, 50.0f)] private float _MovementAcceleration = 5.0f;
    [SerializeField] [Range(1.0f, 50.0f)] private float _LookSpeed = 25.0f;

    private void Awake()
    {
        _PlayerInput = GetComponent<PlayerInput>();
        _MoveAction = _PlayerInput.actions["Move"];
        _LookAction = _PlayerInput.actions["Look"];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _PlayerMovementInput = _MoveAction.ReadValue<Vector3>();
        _Velocity = new Vector3(_PlayerMovementInput.x, 0.0f, _PlayerMovementInput.z) * _MovementAcceleration * Time.deltaTime;
        _lookRotation += _LookAction.ReadValue<float>() * _LookSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0.0f, _lookRotation, 0.0f);
        _Velocity = transform.localRotation * _Velocity;
        transform.position += _Velocity;
    }
}
