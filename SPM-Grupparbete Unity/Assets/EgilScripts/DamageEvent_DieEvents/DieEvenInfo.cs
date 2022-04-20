using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event = EgilEventSystem.Event;

public class DieEvenInfo : Event
{
    public GameObject gameObject;

    public DieEvenInfo(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }
}
