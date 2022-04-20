using UnityEngine;

public class DestroyableWall : DestroyableObjectBase
{
    protected new int materialAmount = 0;
    protected new int materialHP = 5;
    protected new int requiredWeaponLevel = 1;


    public override void ReduceMaterialAmount(int amount)
    {
        if (materialAmount > 0)
        {
            if (amount > materialAmount)
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
            DestroyObject();
        }
    }

    public override int GetRequiredWeaponLevel()
    {
        return requiredWeaponLevel;
    }

    public override int MinedMaterial(int minedMaterial)
    {
        return minedMaterial;
    }

    private void DestroyObject()
    {

        Destroy(this.gameObject);
    }
}
