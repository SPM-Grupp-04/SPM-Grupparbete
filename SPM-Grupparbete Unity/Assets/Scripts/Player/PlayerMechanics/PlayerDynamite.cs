using System;
using System.Collections;
using System.Collections.Generic;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//Main Author: Axel Ingelsson Fredler

public class PlayerDynamite : MonoBehaviour
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
    
    [Header("Camera Shake")]
    [SerializeField] [Range(1.0f, 20.0f)] private float cameraShakeMagnitude;
    [SerializeField] [Range(0.1f, 5.0f)] private float cameraShakeDuration;
    
    [Header("Components")]
    [SerializeField] private GameObject dynamiteExplosionPrefab;
    
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Rigidbody capsuleRigidBody;

    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private Light dynamiteExplosionLight;

    [SerializeField] private AudioSource dynamiteExplosionAudioSource;

    [SerializeField] private AudioSource dynamiteFuseAudioSource;

    [SerializeField] private ParticleSystem dynamiteFuseParticleSystem;

    private FallingRocksSpawner fallingRocksSpawner;

    private GameObject dynamiteExplosion;

    private Vector3 capsulePoint1;
    private Vector3 capsulePoint2;

    private float particleSystemCountdown;
    private float explosionCountdown;

    private bool hasExploded;

    private void Start()
    {
        fallingRocksSpawner = FallingRocksSpawner.Instance;
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
            StartCoroutine(DynamiteExplosion());
            StartCoroutine(PlayParticleSystem());
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

        ExplodeDynamite();
    }

    private void DisableDynamiteFuse()
    {
        dynamiteFuseAudioSource.Stop();
        
        dynamiteFuseParticleSystem.Stop();
    }
    
    private void ExplodeDynamite()
    {
        dynamiteExplosion = Instantiate(dynamiteExplosionPrefab, transform.position, Quaternion.identity);
        
        dynamiteExplosionAudioSource.Play();
        
        dynamiteExplosionLight.enabled = true;

        StartCoroutine(ReduceExplosionLightTime());

        capsuleCollider.enabled = false;
        
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayerMask);
        
        foreach (Collider enemyObject in enemyColliders)
        {
            var damageEvent = new DealDamageEventInfo(enemyObject.gameObject, explosionDamage);
            EventSystem.current.FireEvent(damageEvent);
        }
        
        CameraShake.Instance.ShakeCamera(cameraShakeMagnitude, cameraShakeDuration);
        
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
