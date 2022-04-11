using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    
    private PlayerInput _PlayerInput;
    private InputAction _MoveAction;
    private InputAction Save;
    private InputAction Load;
    private EgilSaveAndLoadImplementation esalI;
    
    private InputAction _JumpAction;
    private Vector3 _Velocity;
    [FormerlySerializedAs("_Acceleration")] [SerializeField] [Range(1.0f, 50.0f)] private float _MovementAcceleration = 5.0f;
    [SerializeField] [Range(1.0f, 10.0f)] private float _JumpForce = 2.0f;

    private void Awake()
    {
        _PlayerInput = GetComponent<PlayerInput>();
        _MoveAction = _PlayerInput.actions["Move"];
        _JumpAction = _PlayerInput.actions["Jump"];
        Save = _PlayerInput.actions["Save"];
        Load = _PlayerInput.actions["Loaded"];
        esalI = GetComponent<EgilSaveAndLoadImplementation>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Så dumt... 
        if (Save.IsPressed())
        {
            esalI.saveGamePress();
        }

        if (Load.IsPressed())
        {
            esalI.LoadGamePress();
        }
        _Velocity = (Vector3)_MoveAction.ReadValue<Vector2>() * _MovementAcceleration;
        transform.position += new Vector3(_Velocity.x, 0.0f, _Velocity.y) * Time.deltaTime;
    }
    
}
