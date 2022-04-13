//Author: Simon Canbäck, sica 4801
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRocksTrapScript : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1.0f), Tooltip("Percentage chance of spawning per second.")] private float spawnThreshold = 0.5f;
    private float SpawnThreshold
    {
        get
        {
            return Mathf.Pow(1.0f - spawnThreshold, Time.deltaTime); //chance per second
        }
    }
    [SerializeField] private GameObject rockPrefab;
    private Bounds bounds;

    // Start is called before the first frame update
    void Start()
    {
        bounds = GetComponent<Collider>().bounds;
        print(SpawnThreshold);
        print(Application.targetFrameRate);
    }

    // Update is called once per frame
    void Update()
    {
        SpawnRock();
    }

    private void SpawnRock()
    {
        if (Random.value < SpawnThreshold)
            return;

        float x = 0.0f;
        float z = 0.0f;

        do
        {
            x = (Random.value - 0.5f) * 2.0f * bounds.extents.x; //[0.0f, 1.0f] -> [-1.0f, 1.0f]
            z = (Random.value - 0.5f) * 2.0f * bounds.extents.z;
        } while (!(bounds.Contains(
            new Vector3(bounds.center.x + x, bounds.center.y, bounds.center.z + z))) //reject coordinates outside bounds
            );

        Instantiate(rockPrefab, new Vector3(bounds.center.x + x, bounds.center.y, bounds.center.z + z), Quaternion.identity).GetComponent<Rigidbody>();
    }
}
