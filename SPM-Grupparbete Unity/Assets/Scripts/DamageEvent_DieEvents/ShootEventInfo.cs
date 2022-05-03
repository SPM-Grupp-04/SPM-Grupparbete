using UnityEngine;
using Event = EgilEventSystem.Event;

namespace EgilScripts.DieEvents
{
    public class ShootEventInfo : Event
    {
        public GameObject target;
        public GameObject ogPos;
        public GameObject throwableObject;
        public float throwCooldown;
        public float throwUpwardForce;
        public float throwForce;
        
        

        public ShootEventInfo(GameObject target,GameObject ogPos, GameObject throwableObject, float throwCooldown, float throwUpwardForce, float throwForce)
        {
            this.target = target;
            this.ogPos = ogPos;
            this.throwableObject = throwableObject;
            this.throwCooldown = throwCooldown;
            this.throwUpwardForce = throwUpwardForce;
            this.throwForce = throwForce;
        }
        
        
        
        


    }
}