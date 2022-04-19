using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event = EgilEventSystem.Event;

public class DieEvenInfo : Event
{
    public GameObject GameObject;

    public DieEvenInfo(GameObject gameObject)
    {
        this.GameObject = gameObject;
    }
}
