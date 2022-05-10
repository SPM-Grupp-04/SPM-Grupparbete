﻿using UnityEngine;
using Event = EgilEventSystem.Event;

namespace EgilScripts.DieEvents
{
    public class ShootEventInfo : Event
    {
       // public GameObject target;
        public Transform ogPos;
        public GameObject throwableObject;
       
        public float throwUpwardForce;
        public float throwForce;
        
        

        public ShootEventInfo(Transform ogPos, GameObject throwableObject,  float throwUpwardForce, float throwForce)
        {
           // this.target = target;
            this.ogPos = ogPos;
            this.throwableObject = throwableObject;
            
            this.throwUpwardForce = throwUpwardForce;
            this.throwForce = throwForce;
        }
        
        
        
        


    }
}