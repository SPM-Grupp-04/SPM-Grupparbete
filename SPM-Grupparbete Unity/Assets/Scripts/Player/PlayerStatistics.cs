//primary author:
//additional programming: Simon Canbäck, sica4801
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
public class PlayerStatistics
{
    private static PlayerStatistics instance = null;

    public float playerMaxHealth = 30;
    public float playerOneHealth = 30;
    public float playerTwoHealth = 30;
    public float playerOneAcceleration = 5.0f;
    public float playerTwoAcceleration = 5.0f;
    public bool playerOneDisco = false;
    public bool playerTwoDisco = false;
    // public string Scene;
    public float PosX, PosY, PosZ;
    public int Crystals;
    public int BlueCrystals;
    public int RedCrystals;
    public int GreenCrystals;
    public int drillLevel = 0;
    public int weaponLevel;
    public int componentsCollectedMask = 0;
    public float shieldCooldownModifer = 1f;
    public Dictionary<string, bool> buttonDictionary;



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



}
