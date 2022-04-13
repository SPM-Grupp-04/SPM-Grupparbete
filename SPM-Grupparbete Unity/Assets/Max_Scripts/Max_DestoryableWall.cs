using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Max_DestoryableWall : Max_DestroyableObjectBase
{
    [SerializeField] int materialAmount = 0;
    [SerializeField] int materialHP = 5;
    [SerializeField] int requierdWeaponLevel = 1;
    [SerializeField] GameObject rock;
    GameObject deleteRock;

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
        Destroy(this.gameObject);
        Instantiate(rock, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), Quaternion.identity);
        
    }
    
}
