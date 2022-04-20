using UnityEngine;
using UnityEngine.InputSystem;
using Event = EgilEventSystem.Event;

namespace EgilScripts.DieEvents
{
    public class DealDamageEventInfo : Event
    {
        // hur mycket skada ska jag göra på det gameobjektet jag träffade.
        public int amountOfDamage;
        public GameObject gameObject;
 
        public DealDamageEventInfo(GameObject gameObject, int amountOfDamage)
        {
            this.amountOfDamage = amountOfDamage;
            this.gameObject = gameObject;
        }


    }
}