using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Max_EnemyTakeDamage : MonoBehaviour
{
    [SerializeField] private int HP = 5;
    private void TakeDamage()
    {
        HP -= 1;
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
