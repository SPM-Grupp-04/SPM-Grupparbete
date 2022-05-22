//Author: Simon Canb�ck, sica4801

using System;
using System.Collections;
using System.Collections.Generic;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;
using UnityEngine.Assertions;

public class FallingRockScript : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private GameObject telegraphMarker;
    [SerializeField, Tooltip("Mark any layers that the rock is supposed to interact with -- most likely Player and Terrain or similar.")] private LayerMask layerMask;
    [SerializeField] private float yDespawnBoundary = -100.0f;

    [SerializeField] private float particleSystemDuration = 5.0f;

    [SerializeField] private ParticleSystem rockExplosionParticleSystem;

    [SerializeField] private MeshRenderer rockMeshRenderer;
    [SerializeField] private MeshRenderer telegraphMarkerMeshRenderer;

    [SerializeField] private SphereCollider rockCollider;

    private float particelSystemCountDown;

    private void Awake()
    {
        particelSystemCountDown = particleSystemDuration;
    }

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsFalse(layerMask == 0);

        RaycastHit hit;

        Physics.Raycast(
            origin: transform.position,
            direction: Physics.gravity.normalized,
            maxDistance: float.PositiveInfinity,
            hitInfo: out hit,
            layerMask: layerMask - (1 << LayerMask.NameToLayer("Player")) //subtract the Player layer from the layer mask
            );

        if (hit.collider == null)
        {
            telegraphMarker.SetActive(false);
            return;
        }

        telegraphMarker.transform.position = hit.point;
        telegraphMarker.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < yDespawnBoundary)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (Utility.LayerMaskExtensions.IsInLayerMask(collision.gameObject, layerMask))
        {
            /*if (collision.gameObject.GetComponent<IDamagable>() != null)
            {
                collision.gameObject.GetComponent<IDamagable>().DealDamage(damage);
            }*/
            var damageEvent = new DealDamageEventInfo(collision.gameObject, 1);
            EventSystem.current.FireEvent(damageEvent);
            
            rockExplosionParticleSystem.Play();

            rockCollider.enabled = false;
            
            rockMeshRenderer.enabled = false;
            telegraphMarkerMeshRenderer.enabled = false;

            StartCoroutine(CountDownToDestruction());
            
        }
    }

    private IEnumerator CountDownToDestruction()
    {
        do
        {
            particelSystemCountDown -= Time.deltaTime;
            yield return null;
        } while (particelSystemCountDown > 0.0f);
        
        Destroy(telegraphMarker);
        Destroy(gameObject);
    }
}
