using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryableWall : DestroyableObjectBase
{
    [SerializeField] int materialAmount = 0;
    [SerializeField] int materialHP = 5;
    [SerializeField] int requierdWeaponLevel = 1;
    [SerializeField] GameObject rubble;


    public override void ReduceMaterialAmount(int amount)
    {
        if(materialAmount > 0)
        {
            if(amount > materialAmount)
            {
                int remaingingMaterials = materialAmount % 0;
                MinedMaterial(remaingingMaterials);
            }
            materialAmount -= amount;
            MinedMaterial(amount);
        }
    }

    public override void ReduceMaterialHP(int amount)
    {
        materialHP -= amount;
        ReduceMaterialAmount(amount);
        Debug.Log("Hit");
        if (materialHP <= 0)
        {
            DestoryObject();
        }
    }

    public override int GetRequierdWeaponLevel()
    {
        return requierdWeaponLevel;
    }

    public override int MinedMaterial(int minedMaterial)
    {
        return minedMaterial;
    }

    private void DestoryObject()
    {
        for(int i = 0; i < 2; i++)
        {
            Instantiate(rubble, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
}
