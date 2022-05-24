//Author: Simon Canb�ck, sica4801
//Additional small changes: Axel Ingelsson Fredler

using System;
using System.Collections;
using System.Collections.Generic;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;
using UnityEngine.Assertions;

public class FallingObjectScript : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private GameObject telegraphMarker;
    [SerializeField, Tooltip("Mark any layers that the rock is supposed to interact with -- most likely Player and Terrain or similar.")] private LayerMask layerMask;
    [SerializeField] private float yDespawnBoundary = -100.0f;

    [SerializeField] private float particleSystemDuration = 5.0f;

    [SerializeField] private ParticleSystem rockExplosionParticleSystem;

    [SerializeField] private AudioSource rockExplosionAudioSource;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < yDespawnBoundary)
            EventSystem.current.FireEvent(new FallingObjectCollided<GameObject>(transform.parent.gameObject));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (Utility.LayerMaskExtensions.IsInLayerMask(collision.gameObject, layerMask))
        {
            var damageEvent = new DamageDealt(collision.gameObject, 1);
            EventSystem.current.FireEvent(damageEvent);

            var collisionEvent = new FallingObjectCollided<GameObject>(transform.parent.gameObject);
            EventSystem.current.FireEvent(collisionEvent);
        }
        else
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }

    private void OnEnable()
    {
        RaycastHit hit;

        Physics.Raycast(
            origin: transform.position,
            direction: Physics.gravity.normalized,
            maxDistance: float.PositiveInfinity,
            hitInfo: out hit,
            layerMask: layerMask - (1 << LayerMask.NameToLayer("Player")) //subtract the Player layer from the layer mask
            );

        telegraphMarker.SetActive(hit.collider != null);

        if(telegraphMarker.activeSelf)
            telegraphMarker.transform.position = hit.point;
    }

    private void OnDisable() => transform.position = transform.parent.position;

    public abstract class FallingObjectCollisionEventBase<T> : EgilEventSystem.Event
    {
        public T CollidingObject
        {
            get; private set;
        }

        public FallingObjectCollisionEventBase(T go)
        {
            CollidingObject = go;
        }
    }

    public class FallingObjectCollided<T> : FallingObjectCollisionEventBase<T>
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
            rockExplosionAudioSource.Play();

            rockCollider.enabled = false;
            
            rockMeshRenderer.enabled = false;
            telegraphMarkerMeshRenderer.enabled = false;

            StartCoroutine(CountDownToDestruction());
            
        }
    }

    public class FallingObjectColliding<T> : FallingObjectCollisionEventBase<T>
    {
        public FallingObjectColliding(T go) : base(go) { }
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
