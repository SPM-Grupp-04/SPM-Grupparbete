using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DestroyableObjectBase : MonoBehaviour
{
    protected int materialHP = -1;
    protected int requiredWeaponLevel = -1;

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

    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }

}
