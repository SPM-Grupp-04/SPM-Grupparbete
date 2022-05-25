using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TownPortal : MonoBehaviour
{
    [Header("Public")]
    public static bool IsTeleporting;
    public Slider slider;
    
    public GameObject loadingScreen;
    
    public Text progressText;
    
    [Header("Private")]
    [SerializeField] private GameObject particalSystem;
    [SerializeField] private GameObject camera;
    
    private GameObject playerOne;
    private GameObject playerTwo;
    private GameObject drone;
    private GameObject copyOfParticleSystem;
    private GameObject transEnable;
    
    private const int HubZoneShopPosition = 1000;
    
    private bool isLoading;
    private void Start()
    {
        playerOne = GameObject.Find("Player1");
        playerTwo = GameObject.Find("Player2");
        drone = GameObject.Find("Drone");
        transEnable = GameObject.Find("Trans");
    }

    private void OnEnable()
    {
        copyOfParticleSystem = Instantiate(particalSystem, transform.position, quaternion.identity);
    }

    private void OnDisable()
    {
        transEnable.SetActive(true);
        Destroy(copyOfParticleSystem);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (!isLoading)
        {
            IsTeleporting = true;
            isLoading = true;
            GlobalControl.SaveData();
            StartCoroutine(LoadAsync(5));

            camera.transform.position = new Vector3(HubZoneShopPosition, camera.transform.position.y, HubZoneShopPosition);

            if (playerOne.activeInHierarchy)
            {
                playerOne.transform.position = new Vector3(HubZoneShopPosition, 3, HubZoneShopPosition); // Hamnar på 800/0/550
            }

            if (playerTwo.activeInHierarchy)
            {
                playerTwo.transform.position = new Vector3(HubZoneShopPosition + 1, 3, HubZoneShopPosition +1);
            }

            drone.transform.position = new Vector3(playerOne.transform.position.x,
                drone.transform.position.y, playerOne.transform.position.z);
            
            StartCoroutine(waitUntillActivate());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isLoading = false;
    }

    private IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        loadingScreen.SetActive(true);
        while (op.isDone == false)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);
            slider.value = progress;
            progressText.text = progress * 100f + "%";
            yield return null;
        }

        StartCoroutine(FakeLoadTime());
    }
    
    private IEnumerator FakeLoadTime()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1);
        loadingScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public static IEnumerator waitUntillActivate()
    {
        yield return new WaitForSeconds(1);
        IsTeleporting = false;
    }
}