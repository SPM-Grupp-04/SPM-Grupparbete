using UnityEngine;

public class DestroyableWall : DestroyableObjectBase
{
    PlayerStatistics playerStatistics = PlayerStatistics.Instance;
    
    [SerializeField] int wallHP = 5;
    [SerializeField] int wallRequiredWeaponLevel = 1;

    private void Awake()
    {
        materialHP = wallHP;
        requiredWeaponLevel = wallRequiredWeaponLevel;
    }

    public override void ReduceMaterialHP(int amount)
    {
        if (playerStatistics.drillLevel >= requiredWeaponLevel)
        {
            materialHP -= amount;
            Debug.Log("Hit");
            if (materialHP <= 0)
            {
                DestroyObject();
            }
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
