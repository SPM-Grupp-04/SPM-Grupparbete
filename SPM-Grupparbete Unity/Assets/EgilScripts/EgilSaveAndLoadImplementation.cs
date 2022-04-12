using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class EgilSaveAndLoadImplementation : MonoBehaviour
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
        
        if (EgilGlobalControl.Instance.IsSceneBeingLoaded)
        {
            EgilGlobalControl.Instance.SavedData = EgilGlobalControl.Instance.SavedData;

            EgilGlobalControl.Instance.IsSceneBeingLoaded = false;
        }
    }
*/

    public void saveGamePress()
    {
        Debug.Log("Pressed O");
        EgilGlobalControl.Instance.SavedData.Scene = SceneManager.GetActiveScene().name;

        EgilGlobalControl.Instance.SaveData();
    }

    public void LoadGamePress()
    {
        Debug.Log("Pressed P");
        EgilGlobalControl.Instance.LoadData();
        EgilGlobalControl.Instance.IsSceneBeingLoaded = true;

        String whichScene = EgilGlobalControl.Instance.SavedData.Scene;
        SceneManager.LoadScene(whichScene);
    }
    /*
    void Update()
    {
        
        if (saveAction.IsPressed())
        {
            Debug.Log("Pressed O");
            EgilGlobalControl.Instance.SavedData.Scene = SceneManager.GetActiveScene().name;

            EgilGlobalControl.Instance.SaveData();
        }
        

        if (loadGame.IsPressed())
        {
            Debug.Log("Pressed P");
            EgilGlobalControl.Instance.LoadData();
            EgilGlobalControl.Instance.IsSceneBeingLoaded = true;

            String whichScene = EgilGlobalControl.Instance.SavedData.Scene;
            SceneManager.LoadScene(whichScene);
        }
        
    }
    */
}