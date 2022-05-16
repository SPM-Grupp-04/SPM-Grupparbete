using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;

public class DealDamageListener : MonoBehaviour
{
    private void Start()
    {
        EventSystem.current.RegisterListener<DamageDealt>(OnDealDamageToUnit);
    }

    void OnDealDamageToUnit(DamageDealt dieEvenInfo)
    {
        if (dieEvenInfo.gameObject.GetComponent<IDamagable>() != null)
        {
            dieEvenInfo.gameObject.GetComponent<IDamagable>().DealDamage(dieEvenInfo.amountOfDamage );
        }
    }
}