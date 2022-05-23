using System;
using UnityEngine;
using UnityEngine.SceneManagement;


    public class Save : MonoBehaviour
    {
        private BoxCollider goal;
        [SerializeField] private int sceneToSwitchTo;
        [SerializeField] private int goalCondition;
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
                    SceneManager.LoadScene(sceneToSwitchTo);
                }
            }
           
        }

       
        
        public void SaveData()
        {
            GlobalControl.SaveData();
        }
        
    }
