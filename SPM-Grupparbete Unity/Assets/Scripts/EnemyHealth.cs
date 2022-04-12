using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int health = 5;


    public void EnemyTakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}