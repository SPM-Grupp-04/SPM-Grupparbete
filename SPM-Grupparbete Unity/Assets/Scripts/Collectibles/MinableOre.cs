using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MinableOre : DestroyableObjectBase
{
    PlayerStatistics playerStatistics = PlayerStatistics.Instance;
    
    [SerializeField] int oreMaterialHP = 10;
    [SerializeField] int oreRequierdWeaponLevel = 1;
    [SerializeField] GameObject ore;
    [SerializeField] GameObject uiHP;
    [SerializeField] [Range(1, 3)] private int collecitbleCrystals = 1;
    [SerializeField] private AudioManager audioManager;

    UI_ObjectHP uiObjectHp;

    private void Start()
    {
        requiredWeaponLevel = oreRequierdWeaponLevel;
        materialHP = oreMaterialHP;
        uiObjectHp = uiHP.GetComponent<UI_ObjectHP>();
        
    }

    public override void ReduceMaterialHP(int amount)
    {
        if (playerStatistics.drillLevel >= requiredWeaponLevel)
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

        audioManager.PlayCrystalSound();
        
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
