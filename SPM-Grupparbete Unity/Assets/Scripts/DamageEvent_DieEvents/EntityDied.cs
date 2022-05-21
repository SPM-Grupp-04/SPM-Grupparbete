using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event = EgilEventSystem.Event;

public class EntityDied : Event
{
    public GameObject gameObject;

    public EntityDied(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }
}
