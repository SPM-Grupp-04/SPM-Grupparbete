using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;

public class DealDamageListener : MonoBehaviour
{
    private EventSystem.EventListener damageListener;

    private void Start()
    {
        damageListener = EventSystem.current.RegisterListener<DamageDealt>(OnDealDamageToUnit);
    }

    void OnDealDamageToUnit(DamageDealt dieEvenInfo)
    {
        if (dieEvenInfo.gameObject.GetComponent<IDamagable>() != null)
        {
            dieEvenInfo.gameObject.GetComponent<IDamagable>().DealDamage(dieEvenInfo.amountOfDamage );
        }
    }

    private void OnDestroy()
    {
        EventSystem.current.UnregisterListener<DamageDealt>(damageListener);
    }
}