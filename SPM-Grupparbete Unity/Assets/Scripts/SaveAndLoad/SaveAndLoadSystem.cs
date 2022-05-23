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
  [Header("When you exit a scen Save and set a scene to load")]
  [SerializeField] private bool save;
  [SerializeField] private int sceneToLoad;
 
  [Header("When You Enter Scene")]
  [SerializeField] private bool load;
    private void Awake()
    {
    }

    private void Start()
    {
        
        if (save)
        {
            saveGamePress();
        }

        if (load)
        {
            LoadGamePress();
        }
    }


    PlayerStatistics playerStatistics = PlayerStatistics.Instance;
  
    private void saveGamePress()
    {
        GlobalControl.Instance.playerStatistics.Crystals = playerStatistics.Crystals;
        GlobalControl.SaveData();
        SceneManager.LoadScene(sceneToLoad);
    }

    private void LoadGamePress()
    {
       
        GlobalControl.Instance.LoadData();
        GlobalControl.Instance.IsSceneBeingLoaded = true;
       
    }
    
}