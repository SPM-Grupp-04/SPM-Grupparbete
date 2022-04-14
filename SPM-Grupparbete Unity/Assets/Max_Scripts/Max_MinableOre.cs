using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Max_MinableOre : Max_DestroyableObjectBase
{
    [SerializeField] int materialAmount;
    [SerializeField] int materialHP = 10;
    [SerializeField] int requierdWeaponLevel = 1;
    [SerializeField] GameObject ore;
    [SerializeField] GameObject uiHP;

    private void Start()
    {
    }

    public override void ReduceMaterialHP(int amount)
    {
       
            materialHP -= amount;
            uiHP.GetComponent<Max_UI_ObjectHP>().OreTakeDamage(amount);
            if (materialHP <= 0)
            {
                DestoryObject();
            }
        
        
    }

    public override int GetRequiredWeaponLevel()
    {
        return requierdWeaponLevel;
    }


    private void DestoryObject()
    {

        int random = Random.Range(1, 3);
        for (int i = 0; i < random; i++)
        {
            Instantiate(ore, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
}
