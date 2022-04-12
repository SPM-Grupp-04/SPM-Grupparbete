using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Max_DestroyableObjectBase : MonoBehaviour
{
    int materialAmount = -1;
    int materialHP = -1;
    int requierdWeaponLevel = -1;


    public virtual void ReduceMaterialAmount(int amount)
    {
        materialAmount -= amount;
    }

    public virtual void ReduceMaterialHP(int amount){
        materialHP -= amount;

        if(materialHP <= 0)
        {
            DestoryObject();
        }
    }

    public virtual int GetRequierdWeaponLevel()
    {
        return requierdWeaponLevel;
    }

    public virtual int MinedMaterial(int amount)
    {
        return -1;
    }

    private void DestoryObject()
    {
        Destroy(this.gameObject);
    }

}
