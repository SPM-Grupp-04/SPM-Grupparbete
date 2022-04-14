using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Max_DestroyableObjectBase : MonoBehaviour
{
    [SerializeField] protected int materialAmount = -1;
    [SerializeField] protected int materialHP = -1;
    [SerializeField] protected int requiredWeaponLevel = -1;


    public virtual void ReduceMaterialAmount(int amount)
    {
        materialAmount -= amount;
    }

    public virtual void ReduceMaterialHP(int amount){
        materialHP -= amount;

        if(materialHP <= 0)
        {
            DestroyObject();
        }
    }

    public virtual int GetRequiredWeaponLevel()
    {
        return requiredWeaponLevel;
    }

    public virtual int MinedMaterial(int amount)
    {
        return -1;
    }

    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }

}
