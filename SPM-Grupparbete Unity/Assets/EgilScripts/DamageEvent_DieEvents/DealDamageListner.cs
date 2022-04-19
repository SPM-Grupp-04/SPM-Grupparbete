using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;

public class DealDamageListner : MonoBehaviour
{
    private void Start()
    {
        EventSystem.current.RegisterListner<DealDamageEventInfo>(OnGiveDamgeToUnit);
    }

    void OnGiveDamgeToUnit(DealDamageEventInfo dieEvenInfo)
    {
        if (dieEvenInfo.GameObject.GetComponent<IDamagable>() != null)
        {
            dieEvenInfo.GameObject.GetComponent<IDamagable>().DealDamage(dieEvenInfo.amountOfDamage);
        }
    }
}