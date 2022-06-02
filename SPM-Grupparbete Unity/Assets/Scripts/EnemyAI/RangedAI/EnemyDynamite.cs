using System;
using System.Collections;
using System.Collections.Generic;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//Main Author: Axel Ingelsson Fredler

public class EnemyDynamite : MonoBehaviour
{
    [Header("Explosion Properties")] 
    [SerializeField] [Range(1.0f, 10.0f)] private float explosionDelay = 3.0f;
    [SerializeField] [Range(1.0f, 20.0f)] private float explosionRadius = 7.5f;
    [SerializeField] [Range(1.0f, 35.0f)] private float explosionDamage = 15.0f;

    [Header("Explosion Layer Masks")] 
    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private LayerMask enemyLayerMask;
    
    [Header("Explosion Light")] 
    [SerializeField] private float explosionLightDuration = 1.0f;

    [Header("Particle System")] 
    [SerializeField] private float particleSystemPlayDuration = 5.0f;

    private float particleSystemCountdown;
    private float explosionCountdown;

    private bool hasExploded;

    [Header("Components")] 
    [SerializeField] private GameObject dynamiteExplosionPrefab;

    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private Rigidbody sphereRigidBody;

    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private Light dynamiteFuseLight;

    [SerializeField] private Light dynamiteExplosionLight;

    [SerializeField] private AudioSource dynamiteExplosionAudioSource;

    private Collider[] playerColliders = new Collider[2];

    private FallingRocksSpawner fallingRocksSpawner;

    private GameObject dynamiteExplosion;

    private Vector3 capsulePoint1;
    private Vector3 capsulePoint2;

    private void Start()
    {
        fallingRocksSpawner = FallingRocksSpawner.Instance;
        explosionCountdown = explosionDelay;
        particleSystemCountdown = particleSystemPlayDuration;
    }

    void FixedUpdate()
    {
        ExplodeAfterDelayAndCollisionWithGround();
    }

    private void ExplodeAfterDelayAndCollisionWithGround()
    {
        if (DynamiteCollidedWithGround() && !hasExploded)
        {
            StartCoroutine(DynamiteExplosion());
            StartCoroutine(PlayParticleSystem());
            hasExploded = true;
        }
    }

    private bool DynamiteCollidedWithGround()
    {
        Physics.SphereCast(transform.position, sphereCollider.radius, sphereRigidBody.velocity.normalized,
            out var sphereCast, Mathf.Infinity, groundLayerMask);
        return sphereCast.collider;
    }

    private IEnumerator DynamiteExplosion()
    {
        do
        {
            explosionCountdown -= Time.deltaTime;
            yield return null;
        } while (explosionCountdown > 0.0f);

        ExplodeDynamiteAndDisableDynamiteFuse();
    }

    private IEnumerator PlayParticleSystem()
    {
        do
        {
            particleSystemCountdown -= Time.deltaTime;
            yield return null;
        } while (particleSystemCountdown > 0.0f);

        DestroyGameObjects();
    }
    
    private IEnumerator ReduceExplosionLightTime()
    {
        float t = 0;
        do
        {
            t += Time.deltaTime * (1.0f / explosionLightDuration);
            dynamiteExplosionLight.intensity = Mathf.Lerp(200.0f, 
                0.0f,
                t);
            yield return null;
        } while (t < 1.0f);

        dynamiteExplosionLight.intensity = 0.0f;
    }

    private void ExplodeDynamiteAndDisableDynamiteFuse()
    {
        DisableDynamiteFuse();

        Explode();
    }

    private void DisableDynamiteFuse()
    {
        dynamiteFuseLight.enabled = false;
    }

    private void Explode()
    {
        dynamiteExplosion = Instantiate(dynamiteExplosionPrefab, transform.position, Quaternion.identity);

        dynamiteExplosionAudioSource.Play();

        dynamiteExplosionLight.enabled = true;
        
        StartCoroutine(ReduceExplosionLightTime());

        sphereCollider.enabled = false;

        Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, playerColliders, enemyLayerMask);
        
        foreach (Collider enemyObject in playerColliders)
        {
            if (enemyObject != null)
            {
                var damageEvent = new DealDamageEventInfo(enemyObject.gameObject, explosionDamage);
                EventSystem.current.FireEvent(damageEvent);   
            }
        }

        fallingRocksSpawner.SetFallingRockAreaPosition(transform.position);
        fallingRocksSpawner.SpawnRocks(true);
        
        meshRenderer.enabled = false;
        
    }

    private void DestroyGameObjects()
    {
        Destroy(dynamiteExplosion);
        Destroy(gameObject);
    }
}
