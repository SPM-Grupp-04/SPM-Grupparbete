using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    /*
     *
     * THIS SCRIPT SHOULD NOT BE USED.
     * 
     */
    private int health = 5;

    private void Update()
    {
        throw new NotImplementedException("THIS SCRIPT SHOULD NOT BE IN USE");
        //Debug.Log("THIS SCRIPT SHOULD NOT BE IN USE!");
    }

    public void EnemyTakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}