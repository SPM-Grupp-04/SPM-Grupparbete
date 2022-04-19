using System.Collections;
using System.Collections.Generic;
using EgilEventSystem;
using UnityEngine;

public class Max_EnemyTakeDamage : MonoBehaviour
{
    [SerializeField] private int HP = 5;
    private void TakeDamage()
    {
        HP -= 1;
        if (HP <= 0)
        {
           // gameObject.SetActive(false);
           var dieEvent = new DieEvenInfo(gameObject);
           
           EventSystem.current.FireEvent(dieEvent);
        }
    }
}
