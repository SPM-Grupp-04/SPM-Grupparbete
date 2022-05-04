using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[Serializable]
public class PlayerStatistics
{

    private static PlayerStatistics instance = null;

    private PlayerStatistics()
    {
    }

    public static PlayerStatistics Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerStatistics();

                //foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
                //{
                //    if (go.GetComponent<EgilHealth>().name == "PlayerOne")
                //        instance.PlayerOneAcceleration = go.GetComponent<PlayerController>().MovementAcceleration;
                //    else if (go.GetComponent<EgilHealth>().name == "PlayerTwo")
                //        instance.PlayerTwoAcceleration = go.GetComponent<PlayerController>().MovementAcceleration;
                        
                //}
            }

            return instance;
        }
    }

    //TODO: Att implemntera ett sätt att göra detta för två spelare.
    // Här kan Samtlig inofrmation om Antal kristaller ligga osv. All form av information som vill spara

    public int PlayerOneHealth = 5;
    public int PlayerTwoHealth = 5;
    public float PlayerOneAcceleration = 5.0f;
    public float PlayerTwoAcceleration = 5.0f;
    public bool PlayerOneDisco = false;
    public bool PlayerTwoDisco = false;
    public string Scene;
    public float PosX, PosY, PosZ;
    public int BlueCrystals;
    public int RedCrystals;

}
