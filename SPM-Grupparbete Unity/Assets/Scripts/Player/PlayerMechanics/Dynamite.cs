using System;
using System.Collections;
using System.Collections.Generic;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;
using UnityEngine.UI;

public class Dynamite : MonoBehaviour
{
    [Header("Explosion Properties")]
    [SerializeField] [Range(1.0f, 10.0f)] private float explosionDelay = 3.0f;
    [SerializeField] [Range(1.0f, 20.0f)] private float explosionRadius = 5.0f;
    [SerializeField] [Range(100.0f, 1000.0f)] private float explosionForce = 500.0f;
    [Header("Explosion Layer Masks")]
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask enemyLayerMask;
    private float explosionCountdown;
    private bool hasExploded;

    private CapsuleCollider capsuleCollider;
    private Rigidbody capsuleRigidBody;
    
    private Vector3 capsulePoint1;
    private Vector3 capsulePoint2;
    
    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleRigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        explosionCountdown = explosionDelay;
    }

    void Update()
    {
        ExplodeAfterDelayAndCollisionWithGround();
    }

    private void ExplodeAfterDelayAndCollisionWithGround()
    {
        if (DynamiteCollidedWithGround() && !hasExploded)
        {
            StartCoroutine(CountdownToExplode());
            hasExploded = true;
        }
    }
    private bool DynamiteCollidedWithGround()
    {
        capsulePoint1 = (capsuleCollider.center + Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius)) + transform.position;
        capsulePoint2 = (capsuleCollider.center + Vector3.down * (capsuleCollider.height / 2 - capsuleCollider.radius)) + transform.position;
        Physics.CapsuleCast(capsulePoint2, capsulePoint1, capsuleCollider.radius, capsuleRigidBody.velocity.normalized, out var capsuleCast, Mathf.Infinity, groundLayerMask);
        return capsuleCast.collider;
    }

    private IEnumerator CountdownToExplode()
    {
        do
        {
            explosionCountdown -= Time.deltaTime;
            yield return null;
        } while (explosionCountdown > 0.0f);
        Explode();
    }

    private void Explode()
    {
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayerMask);
        foreach (Collider enemyObject in enemyColliders)
        {
            Debug.Log(enemyObject.gameObject.name);
            var damageEvent = new DealDamageEventInfo(enemyObject.gameObject, 2);
            EventSystem.current.FireEvent(damageEvent);
        }
    }
}
