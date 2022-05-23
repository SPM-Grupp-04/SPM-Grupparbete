using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;

public class DealDamageListener : MonoBehaviour
{
    private void Start()
    {
        EventSystem.current.RegisterListner<DealDamageEventInfo>(OnDealDamageToUnit);
    }

    void OnDealDamageToUnit(DealDamageEventInfo dieEvenInfo)
    {
        if (dieEvenInfo.gameObject.GetComponent<IDamagable>() != null)
        {
            dieEvenInfo.gameObject.GetComponent<IDamagable>().DealDamage(dieEvenInfo.amountOfDamage );
        }
    }
}