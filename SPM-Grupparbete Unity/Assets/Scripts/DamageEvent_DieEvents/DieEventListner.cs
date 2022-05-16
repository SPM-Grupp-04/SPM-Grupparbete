using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EgilEventSystem;

public class DieEventListner : MonoBehaviour
{
    private void Start()
    {
        EventSystem.current.RegisterListener<DieEvenInfo>(OnUnitDied);
    }

    void OnUnitDied(DieEvenInfo dieEvenInfo)
    {
        //Debug.Log(dieEvenInfo.GameObject.name + " Has died");
        dieEvenInfo.gameObject.SetActive(false);

       
        
    }
}
