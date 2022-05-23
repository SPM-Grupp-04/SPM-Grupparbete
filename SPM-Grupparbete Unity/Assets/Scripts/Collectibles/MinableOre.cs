//Additional authors: Simon Canbäck, sica4801
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
    [SerializeField] [Range(1, 3)] private int collectibleCrystals = 1;

    private EventSystem.EventListener damageListener;

    UI_ObjectHP uiObjectHp;

    private void Start()
    {
        requiredWeaponLevel = oreRequiredWeaponLevel;
        materialHP = oreMaterialHP;
        uiObjectHp = uiHP.GetComponent<UI_ObjectHP>();

        //damageListener = EventSystem.current.RegisterListener<DamageDealt>(OnDamage);
    }

    private void OnDamage(DamageDealt dd)
    {
        if (dd.gameObject == gameObject)
        {
            ReduceMaterialHP((int)dd.amountOfDamage);
        }
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
        for (int i = 0; i < collectibleCrystals; i++)
        {
            Instantiate(ore, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), Quaternion.identity);
        }

        Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        damageListener = EventSystem.current.RegisterListener<DamageDealt>(OnDamage);
    }

    //ensures no dangling references remain
    private void OnDisable()
    {
        if (damageListener != null && EventSystem.current != null)  //needed to ensure the game does not generate exceptions on application quit
        {
            EventSystem.current.UnregisterListener<DamageDealt>(damageListener);
            damageListener = null;
        }
    }

    public int GetOreMaterialHP()
    {
        return oreMaterialHP;
    }
}
