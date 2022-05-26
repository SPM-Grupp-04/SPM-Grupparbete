using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OreCollection : MonoBehaviour
{
    [SerializeField] string oreName = "";
    [SerializeField] private float minModifier, maxModifier;

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

//    private void OnTriggerStay(Collider other)
    //   {
    //      if (!other.CompareTag("Player")) return;

    //      transform.position = Vector3.SmoothDamp(transform.position, other.gameObject.transform.position, ref velocity,
    //          Time.deltaTime * Random.Range(minModifier, maxModifier));
    //  }

    private void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        //  Vector3.SmoothDamp(transform.position, target.transform.position, ref velocity,
        //   Time.deltaTime * Random.Range(minModifier, maxModifier));
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