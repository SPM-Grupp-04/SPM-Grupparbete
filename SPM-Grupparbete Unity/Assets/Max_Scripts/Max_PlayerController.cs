using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Max_PlayerController : MonoBehaviour
{
    private PlayerInput _PlayerInput;
    private InputAction _MoveAction;
    private InputAction _JumpAction;
    private InputAction _drillAction;
    private InputAction _shootAction;
    private GameObject _drill;

    private Vector3 _Velocity;
    [FormerlySerializedAs("_Acceleration")] [SerializeField] [Range(1.0f, 50.0f)] private float _MovementAcceleration = 5.0f;
    [SerializeField] [Range(1.0f, 10.0f)] private float _JumpForce = 2.0f;

    private void Awake()
    {
        _PlayerInput = GetComponent<PlayerInput>();
        _MoveAction = _PlayerInput.actions["Move"];
        _JumpAction = _PlayerInput.actions["Jump"];
        _drillAction = _PlayerInput.actions["Drill"];
        _shootAction = _PlayerInput.actions["Shoot"];
        _drill = transform.Find("Drill").gameObject;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _Velocity = (Vector3)_MoveAction.ReadValue<Vector2>() * _MovementAcceleration;
        transform.position += new Vector3(_Velocity.x, 0.0f, _Velocity.y) * Time.deltaTime;

        //if (inputAction.WasPerformedThisFrame())
        if (_shootAction.IsPressed())
        {
            _drill.gameObject.SendMessage("Shoot");
            _drill.gameObject.SendMessage("DrillInUse", true);
        }
        else
        {
            if (_drillAction.IsPressed())
            {
                _drill.gameObject.SendMessage("DrillObject");
                _drill.gameObject.SendMessage("DrillInUse", true);
            }
            else
            {
                _drill.gameObject.SendMessage("DrillInUse", false);
            }
        }

    }
}
