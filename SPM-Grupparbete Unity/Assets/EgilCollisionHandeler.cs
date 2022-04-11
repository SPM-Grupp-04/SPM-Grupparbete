using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgilCollisionHandeler : MonoBehaviour
{
    private EgilHealth eh;
    private void Start()
    {
        eh = GetComponent<EgilHealth>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("Enemy"))
        {
            eh.Takedamage();
        }
    }
}