using System;
using System.Collections;
using System.Collections.Generic;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Dynamite : MonoBehaviour
{
    [Header("Explosion Properties")]
    [SerializeField] [Range(1.0f, 10.0f)] private float explosionDelay = 3.0f;
    [SerializeField] [Range(1.0f, 20.0f)] private float explosionRadius = 5.0f;
    [Header("Explosion Layer Masks")]
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask enemyLayerMask;
    [Header("Particle System")]
    [SerializeField] private float particleSystemPlayDuration = 5.0f;
    private float particleSystemCountdown;
    private float explosionCountdown;
    private bool hasExploded;

    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Rigidbody capsuleRigidBody;

    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private GameObject dynamiteExplosionPrefab;

    private FallingRocksSpawner fallingRocksSpawner;

    private Vector3 capsulePoint1;
    private Vector3 capsulePoint2;

    private void Awake()
    {
        fallingRocksSpawner = FallingRocksSpawner.Instance;
    }

    private void Start()
    {
        explosionCountdown = explosionDelay;
        particleSystemCountdown = particleSystemPlayDuration;
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
        GameObject dynamiteExplosion = Instantiate(dynamiteExplosionPrefab, transform.position, Quaternion.identity);
        //Gamepad.current.SetMotorSpeeds(1,0);
        capsuleCollider.enabled = false;
        Explode();
        do
        {
            particleSystemCountdown -= Time.deltaTime;
            yield return null;
        } while (particleSystemCountdown > 0.0f);
        
        //Gamepad.current.SetMotorSpeeds(0,0);
        Destroy(dynamiteExplosion);
        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayerMask);
        foreach (Collider enemyObject in enemyColliders)
        {
            var damageEvent = new DealDamageEventInfo(enemyObject.gameObject, 5);
            EventSystem.current.FireEvent(damageEvent);
        }
        fallingRocksSpawner.SetFallingRockAreaPosition(transform.position);
        fallingRocksSpawner.SpawnRocks(true);
        meshRenderer.enabled = false;
    }
}
