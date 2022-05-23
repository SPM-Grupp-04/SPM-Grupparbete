using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Save : MonoBehaviour
    {
        private BoxCollider goal;
        [SerializeField] private int sceneToSwitchTo;
        [SerializeField] private int goalCondition;
        
        public GameObject loadingScreen;
        public Slider Slider;
        public Text progressText;
        private void Awake()
        {
            goal = GetComponent<BoxCollider>();
        }

        PlayerStatistics playerStatistics = PlayerStatistics.Instance;

        private void OnCollisionEnter(Collision collision)
        {

            if (collision.gameObject.CompareTag("Player")) 
            {
                SaveData();
            
                if (PlayerStatistics.Instance.componentsCollectedMask >= goalCondition)
                {
                   
                    //SceneManager.LoadScene(sceneToSwitchTo);
                    StartCoroutine(LoadAsynchronusly(sceneToSwitchTo));
                }
            }
           
        }
        
        IEnumerator LoadAsynchronusly(int sceneIndex)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

            loadingScreen.SetActive(true);
            while (operation.isDone == false)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);
                Slider.value = progress;
                progressText.text = progress * 100f + "%";
                yield return null;
            }
        }

       
        
        public void SaveData()
        {
            GlobalControl.SaveData();
        }
        
    }
