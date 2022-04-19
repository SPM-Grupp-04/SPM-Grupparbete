using System;
using UnityEngine;

namespace EgilScripts.EnemyAI
{
    public class Bullet : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer.Equals("player"))
            {
                Debug.Log("Hit a player");
            }
            Destroy(gameObject);
        }
    }
}