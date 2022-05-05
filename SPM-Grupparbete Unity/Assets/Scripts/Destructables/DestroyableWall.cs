using UnityEngine;

public class DestroyableWall : DestroyableObjectBase
{
    PlayerStatistics playerStatistics = PlayerStatistics.Instance;
    
    [SerializeField] int wallHP = 5;
    [SerializeField] int wallRequiredWeaponLevel = 1;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject uiHP;
    UI_ObjectHP uiObjectHp;

    private void Awake()
    {
        materialHP = wallHP;
        requiredWeaponLevel = wallRequiredWeaponLevel;
        uiObjectHp = uiHP.GetComponent<UI_ObjectHP>();

    }

    public override void ReduceMaterialHP(int amount)
    {
        if (playerStatistics.drillLevel >= requiredWeaponLevel)
        {
            materialHP -= amount;
            uiObjectHp.ObjectTakeDamage(amount);
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

    public int GetWallHP()
    {
        return materialHP;
    }
}
