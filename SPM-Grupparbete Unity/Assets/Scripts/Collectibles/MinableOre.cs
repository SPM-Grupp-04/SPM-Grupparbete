using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinableOre : DestroyableObjectBase
{
    PlayerStatistics playerStatistics = PlayerStatistics.Instance;
    
    [SerializeField] int oreMaterialHP = 10;
    [SerializeField] int oreRequierdWeaponLevel = 1;
    [SerializeField] GameObject ore;
    [SerializeField] GameObject uiHP;

    private void Start()
    {
        requiredWeaponLevel = oreRequierdWeaponLevel;
        materialHP = oreMaterialHP;

    }

    public override void ReduceMaterialHP(int amount)
    {
        if (playerStatistics.drillLevel >= requiredWeaponLevel)
        {
            materialHP -= amount;
            uiHP.GetComponent<UI_ObjectHP>().OreTakeDamage(amount);
            if (materialHP <= 0)
            {
                DestoryObject();
            }
        }
    }

    public override int GetRequiredWeaponLevel()
    {
        return requiredWeaponLevel;
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

    public int GetOreMaterialHP()
    {
        return oreMaterialHP;
    }
}
