using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TownPortal : MonoBehaviour
{
    private GameObject playerOne;
    private GameObject playerTwo;
    private GameObject drone;
    [SerializeField]   private GameObject camera;
    private bool isLoading;
    public static bool isTeleporting;
  
    
    public GameObject loadingScreen;
    public Slider Slider;
    public Text progressText;

    private void Start()
    {
        playerOne = GameObject.Find("Player1");
        playerTwo = GameObject.Find("Player2");
        drone = GameObject.Find("Drone");
    }

    [SerializeField] private GameObject particalSystem;
    private GameObject copyOfParticalSystem;
    private void OnEnable()
    {
       copyOfParticalSystem = Instantiate(particalSystem,transform.position,quaternion.identity);
    }

    private void OnDisable()
    {
        Destroy(copyOfParticalSystem);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        if (!isLoading)
        {
           
            isTeleporting = true;
            isLoading = true;
            GlobalControl.SaveData();
            //SceneManager.LoadScene(5, LoadSceneMode.Additive);
            StartCoroutine(LoadAsynchronusly(5));
         
            camera.transform.position = new Vector3(1000, camera.transform.position.y, 1000);

            if (playerOne.activeInHierarchy)
            {
                playerOne.transform.position = new Vector3(1000, 3, 1000); // Hamnar på 800/0/550
            }

            if (playerTwo.activeInHierarchy)
            {
                playerTwo.transform.position = new Vector3(1001, 3, 1001);
            }
           
            drone.transform.position =new Vector3(playerOne.transform.position.x,
                drone.transform.position.y, playerOne.transform.position.z);

           
            StartCoroutine(waitUntillActivate());
            
           
           
            
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
       
        isLoading = false;
    }

    IEnumerator LoadAsynchronusly(int sceneIndex)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
      
        loadingScreen.SetActive(true);
        while (op.isDone == false)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);
            Slider.value = progress;
            progressText.text = progress * 100f + "%";
            yield return null;
        }
        loadingScreen.SetActive(false);
    }

   public static IEnumerator waitUntillActivate()
    {
        yield return new  WaitForSeconds(1);
        isTeleporting = false;
       

    }
}