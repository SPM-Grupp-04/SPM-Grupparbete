using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class EgilEnemyDetectionBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _enemyTransform;
    [SerializeField] [Range(1.0f, 50.0f)] private float _enemyMovementSpeed = 2.5f;
    [SerializeField] private LayerMask _layerMask;

    private SphereCollider c;
    private Collider[] colliders;
    private GameObject player;

    private Vector3 _playerDistance;

    private const float LARGENUMBER = 100;

    private float tempHealth = LARGENUMBER;

    private void Start()
    {
        c = GetComponent<SphereCollider>();
    }


    private void Update()
    {
        CheckForPlayer();

        if (player != null)
        {
            StartFollowPlayer();
        }

        StopFollowPlayer();
    }

    private void CheckForPlayer()
    {
        colliders = Physics.OverlapSphere(transform.position + c.center, c.radius, _layerMask);


        foreach (var col in colliders)
        {
            EgilHealth egh = col.GetComponent<EgilHealth>();

            if (egh.localEgilPlayerData.PlayerOnehp < tempHealth)
            {
                player = col.gameObject;
                tempHealth = egh.localEgilPlayerData.PlayerOnehp;
            }
        }

        tempHealth = LARGENUMBER;
    }

    private void StartFollowPlayer()
    {
        _playerDistance = player.transform.position - _enemyTransform.position;
        _enemyTransform.position += _playerDistance.normalized * Time.deltaTime * _enemyMovementSpeed;
        _enemyTransform.rotation = Quaternion.LookRotation(-_playerDistance.normalized, Vector3.up);
    }

    private void StopFollowPlayer()
    {
        if (_playerDistance != null && _playerDistance.magnitude > 10)
        {
            player = null;
        }
    }


    /*
    private void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            //Vector3.MoveTowards(_enemyTransform.position, other.transform.position, _enemyMovementSpeed);
            Vector3 _playerDistance = other.transform.position - _enemyTransform.position;
            _enemyTransform.position += _playerDistance.normalized * Time.deltaTime * _enemyMovementSpeed;
            _enemyTransform.rotation = Quaternion.LookRotation(-_playerDistance.normalized, Vector3.up);
        }
    }
    */
}