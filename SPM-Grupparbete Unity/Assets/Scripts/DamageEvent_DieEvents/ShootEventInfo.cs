using UnityEngine;
using Event = EgilEventSystem.Event;

namespace EgilScripts.DieEvents
{
    public class ShootEventInfo : Event
    {
        public GameObject target;
        public GameObject ogPos;

        public ShootEventInfo(GameObject target,GameObject ogPos)
        {
            this.target = target;
            this.ogPos = ogPos;
        }
        
        


    }
}