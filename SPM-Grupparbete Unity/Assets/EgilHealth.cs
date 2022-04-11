using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EgilHealth : MonoBehaviour
{
    [SerializeField] [Range(1, 10)] private float health = 5;
    
    void Update()
    {
        if (health < 1)
        {
            Destroy(transform.gameObject);
        }
    }

    public void Takedamage()
    {
        health--;
    }
    
    
}