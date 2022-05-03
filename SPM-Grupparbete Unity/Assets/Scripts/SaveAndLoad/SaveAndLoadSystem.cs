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

    // Start is called before the first frame update
  /*
    void Start()
    {
        
        PI = GetComponent<PlayerInput>();
        
        saveAction = PI.actions["Save"];
        loadGame = PI.actions["Loaded"];
        
        if (GlobalControl.Instance.IsSceneBeingLoaded)
        {
            GlobalControl.Instance.SavedData = GlobalControl.Instance.SavedData;

            GlobalControl.Instance.IsSceneBeingLoaded = false;
        }
    }
*/

    public void saveGamePress()
    {
        Debug.Log("Pressed O");
        GlobalControl.Instance.SavedData.Scene = SceneManager.GetActiveScene().name;

        GlobalControl.Instance.SaveData();
    }

    public void LoadGamePress()
    {
        Debug.Log("Pressed P");
        GlobalControl.Instance.LoadData();
        GlobalControl.Instance.IsSceneBeingLoaded = true;

        String whichScene = GlobalControl.Instance.SavedData.Scene;
        SceneManager.LoadScene(whichScene);
    }
    /*
    void Update()
    {
        
        if (saveAction.IsPressed())
        {
            Debug.Log("Pressed O");
            GlobalControl.Instance.SavedData.Scene = SceneManager.GetActiveScene().name;

            GlobalControl.Instance.SaveData();
        }
        

        if (loadGame.IsPressed())
        {
            Debug.Log("Pressed P");
            GlobalControl.Instance.LoadData();
            GlobalControl.Instance.IsSceneBeingLoaded = true;

            String whichScene = GlobalControl.Instance.SavedData.Scene;
            SceneManager.LoadScene(whichScene);
        }
        
    }
    */
}