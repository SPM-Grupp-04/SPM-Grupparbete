//Author: Simon Canb�ck, sica 4801
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using EgilEventSystem;

public class FallingObjectsTrapScript : MonoBehaviour
{
    private float SpawnThreshold => Mathf.Pow(1.0f - spawnThreshold, Time.deltaTime); //chance per second
    [SerializeField, Range(0.1f, 1.0f), Tooltip("Percentage chance of spawning per second.")] private float spawnThreshold = 0.5f;
    [SerializeField, Tooltip("Use x and z to set up a \"sideways\" trap!")] private Vector3 objectStartingForce = Vector3.down;
    [SerializeField, Tooltip("Adds some variance in how fast objects fall. Recommended for when spawnThreshold is close to 1.")] private Vector2 randomStartingForceMultiplierRange = new Vector2(1.0f, 20.0f);

    [SerializeField] private GameObject objectPrefab;
    private Bounds bounds;
    protected ObjectPool<GameObject> objectPool;
    [SerializeField] protected int poolSize = 20;

    private EventSystem.EventListener collidedListener;

    // Start is called before the first frame update
    void Start()
    {
        bounds = GetComponent<Collider>().bounds;

        objectPool = new ObjectPool<GameObject>(
                createFunc: CreateObject,
                actionOnGet: (go) => go.SetActive(true),
                actionOnRelease: (go) => go.SetActive(false),
                defaultCapacity: poolSize,
                maxSize: poolSize
            );

        Stack<GameObject> gos = new Stack<GameObject>();

        for (int i = 0; i < poolSize; i++)
            gos.Push(objectPool.Get());

        for (int i = 0; i < poolSize; i++)
            objectPool.Release(gos.Pop());
    }

    // Update is called once per frame
    void Update()
    {
        SpawnFallingObject();
    }

    protected virtual GameObject CreateObject()
    {
        //guard rail in case a lot of objects get created but not released
        if (objectPool.CountAll > poolSize)
            return null;

        GameObject go = Instantiate(objectPrefab, transform.localPosition, Quaternion.identity);
        go.transform.parent = transform;
        go.SetActive(false);

        return go;
    }

    private void SpawnFallingObject()
    {
        if (Random.value < SpawnThreshold)
            return;

        float x = 0.0f;
        float z = 0.0f;

        do
        {
            x = (Random.value - 0.5f) * 2.0f * bounds.extents.x; //[0.0f, 1.0f] * extents -> [-1.0f, 1.0f] * extents
            z = (Random.value - 0.5f) * 2.0f * bounds.extents.z;
        } while (!(bounds.Contains(
            new Vector3(bounds.center.x + x, bounds.center.y, bounds.center.z + z))) //reject coordinates outside bounds
            );

        GameObject go = objectPool.Get();

        if (go == null)
            return;

        go.transform.position = new Vector3(bounds.center.x + x, bounds.center.y, bounds.center.z + z);
        go.GetComponentInChildren<Rigidbody>().AddForce(objectStartingForce * Random.Range(randomStartingForceMultiplierRange.x, randomStartingForceMultiplierRange.y));
    }

    private void OnEnable()
    {
        collidedListener = EventSystem.current.RegisterListener<FallingObjectScript.FallingObjectCollided<GameObject>>(
            (foce) =>
                {
                    if (foce.CollidingObject.transform.parent == transform)
                        objectPool.Release(foce.CollidingObject);
                }
            );
    }

    private void OnDisable()
    {
        if (collidedListener != null && EventSystem.current != null)
        {
            EventSystem.current.UnregisterListener<FallingObjectScript.FallingObjectCollided<GameObject>>(collidedListener);
            collidedListener = null;
        }
    }
}
