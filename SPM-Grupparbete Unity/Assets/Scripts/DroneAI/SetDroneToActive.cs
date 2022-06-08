using System;
using UnityEngine;

public class SetDroneToActive : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(GlobalControl.Instance.playerStatistics.buttonDictionary["DroneButton"]);
    }
}