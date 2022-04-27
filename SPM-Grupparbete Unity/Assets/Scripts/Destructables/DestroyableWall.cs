using UnityEngine;

public class DestroyableWall : DestroyableObjectBase
{
    [SerializeField] int WallHP = 5;
    [SerializeField] int WallRequiredWeaponLevel = 1;

    private void Awake()
    {
        materialHP = WallHP;
        requiredWeaponLevel = WallRequiredWeaponLevel;
    }

    public override void ReduceMaterialHP(int amount)
    {
        materialHP -= amount;
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

    private void DestroyObject()
    {

        Destroy(this.gameObject);
    }
}
