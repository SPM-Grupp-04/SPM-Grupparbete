using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class SaveAndLoadSystem : MonoBehaviour
{
  /*
    private PlayerInput PI;

    private InputAction saveAction;
    private InputAction loadGame;
*/
    private void Awake()
    {
    }


    PlayerStatistics playerStatistics = PlayerStatistics.Instance;
  
    public void saveGamePress()
    {
        Debug.Log("Pressed O");
      //  GlobalControl.Instance.playerStatistics.Scene = SceneManager.GetActiveScene().name;
        GlobalControl.Instance.playerStatistics.Crystals = playerStatistics.Crystals;
        
        GlobalControl.Instance.SaveData();
    }

    public void LoadGamePress()
    {
        Debug.Log("Pressed P");
        GlobalControl.Instance.LoadData();
        GlobalControl.Instance.IsSceneBeingLoaded = true;
       
    }
    
}