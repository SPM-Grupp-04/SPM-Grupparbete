using System;
using UnityEngine;

public class SetDroneToActive : MonoBehaviour
{
    private void Start()
    {
        if (GlobalControl.Instance.playerStatistics.buttonDictionary != null)
        {
            gameObject.SetActive(GlobalControl.Instance.playerStatistics.buttonDictionary["DroneButton"]);
        }
    }

    public void SetDronesActive()
    {
        gameObject.SetActive(GlobalControl.Instance.playerStatistics.buttonDictionary["DroneButton"]);

    }
}