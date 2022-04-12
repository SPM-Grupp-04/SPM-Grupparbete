using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _enemyTransform;
    [SerializeField] [Range(1.0f, 50.0f)] private float _enemyMovementSpeed = 2.5f;

    


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

   
}
