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
    
    [Header("Explosion Layer Masks")]
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask enemyLayerMask;
    
    [Header("Particle System")]
    [SerializeField] private float particleSystemPlayDuration = 5.0f;
    
    private float particleSystemCountdown;
    private float explosionCountdown;
    
    private bool hasExploded;
    
    [Header("Components")]
    [SerializeField] private GameObject dynamiteExplosionPrefab;
    
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Rigidbody capsuleRigidBody;

    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private Light dynamiteFuseLight;

    [SerializeField] private Light dynamiteExplosionLight;

    [SerializeField] private AudioSource dynamiteExplosionAudioSource;

    [SerializeField] private AudioSource dynamiteFuseAudioSource;

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
        dynamiteFuseAudioSource.Play();
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

        dynamiteFuseLight.enabled = false;
        
        dynamiteFuseAudioSource.Stop();
        
        GameObject dynamiteExplosion = Instantiate(dynamiteExplosionPrefab, transform.position, Quaternion.identity);
        
        dynamiteExplosionAudioSource.Play();
        
        dynamiteExplosionLight.enabled = true;
        capsuleCollider.enabled = false;
        
        Explode();
        
        do
        {
            particleSystemCountdown -= Time.deltaTime;
            yield return null;
        } while (particleSystemCountdown > 0.0f);
        
        Destroy(dynamiteExplosion);
        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayerMask);
        foreach (Collider enemyObject in enemyColliders)
        {
            if (!enemyObject.gameObject.GetComponent<PlayerController>().InsideShield)
            {
                var damageEvent = new DealDamageEventInfo(enemyObject.gameObject, 10);
                EventSystem.current.FireEvent(damageEvent);
            }
        }
        fallingRocksSpawner.SetFallingRockAreaPosition(transform.position);
        fallingRocksSpawner.SpawnRocks(true);
        meshRenderer.enabled = false;
    }
}
