using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EgilEventSystem;
using EgilScripts.DieEvents;

public class MinableOre : DestroyableObjectBase
{
    PlayerStatistics playerStatistics = PlayerStatistics.Instance;

    [SerializeField] int oreMaterialHP = 10;
    [SerializeField] int oreRequiredWeaponLevel = 1;
    [SerializeField] GameObject ore;
    [SerializeField] GameObject uiHP;
    [SerializeField] [Range(1, 3)] private int collecitbleCrystals = 1;

    UI_ObjectHP uiObjectHp;

    private void Start()
    {
        requiredWeaponLevel = oreRequiredWeaponLevel;
        materialHP = oreMaterialHP;
        uiObjectHp = uiHP.GetComponent<UI_ObjectHP>();

        EventSystem.current.RegisterListener<DamageDealt>(
            (dd) =>
                {
                    if (dd.gameObject == gameObject)
                        ReduceMaterialHP((int)dd.amountOfDamage);
                }
            );
    }

    public override void ReduceMaterialHP(int amount)
    {
        if ((int)playerStatistics.armamentLevel >= requiredWeaponLevel)
        {
            materialHP -= amount;
            uiObjectHp.ObjectTakeDamage(amount);

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
        for (int i = 0; i < collecitbleCrystals; i++)
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
