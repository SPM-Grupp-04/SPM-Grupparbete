using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EgilEventSystem;

public class EntityDiedListener : MonoBehaviour
{
    private EventSystem.EventListener deathListener;

    private void Start()
    {
        deathListener = EventSystem.current.RegisterListener<EntityDied>(OnUnitDied);
    }

    void OnUnitDied(EntityDied dieEvenInfo)
    {
        dieEvenInfo.gameObject.SetActive(false);
        EventSystem.current.UnregisterListener<EntityDied>(deathListener);
    }
}
