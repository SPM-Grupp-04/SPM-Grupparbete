//Author: Simon Canb�ck, sica 4801

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class FallingRocksTrapScript : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1.0f), Tooltip("Percentage chance of spawning per second.")] private float spawnThreshold = 0.5f;
    [SerializeField] private Vector3 rockStartingForce;

    [SerializeField] private float rockSpawnDuration = 3.0f;

    [SerializeField] [Range(1.0f, 20.0f)] private float cameraShakeMagnitude;
    [SerializeField] [Range(0.1f, 5.0f)] private float cameraShakeDuration;

    private float rockSpawnCountDown;
    
    [SerializeField] private GameObject rockPrefab;
    private Bounds bounds;

    [SerializeField] private Collider spawnArea;

    private CameraShake cameraShake;

    private Vector3 cameraFollowPoint;

    private static FallingRocksTrapScript instance;
    
    public static FallingRocksTrapScript Instance
    {
        get { return instance; }
    }

    private bool spawnRocks;
    

    public bool SpawningRocks()
    {
        return spawnRocks;
    }

    public void SpawnRocks(bool spawnRocksState)
    {
        spawnRocks = spawnRocksState;
    }
    private float SpawnThreshold
    {
        get
        {
            return Mathf.Pow(1.0f - spawnThreshold, Time.deltaTime); //chance per second
        }
    }
    private void Awake()
    {
        cameraShake = CameraShake.Instance;
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //bounds = GetComponent<Collider>().bounds;
       // print(SpawnThreshold);
        //print(Application.targetFrameRate);
    }

    public void SetFallingRockAreaPosition(Vector3 fallingRockAreaPosition)
    {
        transform.position = fallingRockAreaPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnRocks)
        {
            StartCoroutine(SpawnRocksForDuration());
            cameraShake.ShakeCamera(cameraShakeMagnitude, cameraShakeDuration);
            spawnRocks = false;
        }
    }

    private IEnumerator SpawnRocksForDuration()
    {
        rockSpawnCountDown = rockSpawnDuration;
        do
        {
            SpawnRock();
            rockSpawnCountDown -= Time.deltaTime;
            yield return null;
        } while (rockSpawnCountDown > 0.0f);
    }

    private void SpawnRock()
    {
        if (Random.value * 1.1f < SpawnThreshold)
            return;

        float x = transform.position.x;
        float z = transform.position.z;

        do
        {
            x = (Random.value - 0.5f) * 2.0f * spawnArea.bounds.extents.x; //[0.0f, 1.0f] -> [-1.0f, 1.0f]
            z = (Random.value - 0.5f) * 2.0f * spawnArea.bounds.extents.z;
        } while (!(spawnArea.bounds.Contains(
            new Vector3(spawnArea.bounds.center.x + x, spawnArea.bounds.center.y, spawnArea.bounds.center.z + z))) //reject coordinates outside bounds
            );

        GameObject tempRock = Instantiate(rockPrefab, new Vector3(spawnArea.bounds.center.x + x, spawnArea.bounds.center.y, spawnArea.bounds.center.z + z), Quaternion.identity);
        tempRock.GetComponent<Rigidbody>().AddForce(rockStartingForce);
    }
}
