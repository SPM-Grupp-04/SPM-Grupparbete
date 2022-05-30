using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OreCollection : MonoBehaviour
{
    [SerializeField] string oreName = "";
    [SerializeField] private float minModifier, maxModifier;
    [SerializeField] private AudioManager audioManager;

    private float speed = 7;
    private Vector3 velocity = Vector3.zero;
    private GameObject target;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (target == null)
        {
            target = other.gameObject;
            speed = Random.Range(minModifier, maxModifier);
        }
    }
    private void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    public void CollectOre()
    { 
        
        DestroyObject();
    }

    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }

    public string GetName()
    {
        return oreName;
    }
}