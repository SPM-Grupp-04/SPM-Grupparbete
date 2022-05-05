using System;
using UnityEngine;
using UnityEngine.SceneManagement;


    public class Save : MonoBehaviour
    {
        private BoxCollider goal;
        [SerializeField] private int sceneToSwitchTo;
        private void Awake()
        {
            goal = GetComponent<BoxCollider>();
        }

        PlayerStatistics playerStatistics = PlayerStatistics.Instance;
        
        private void OnTriggerEnter(Collider other)
        {
            SaveData();
            SceneManager.LoadScene(sceneToSwitchTo);
        }

        
        public void SaveData()
        {
            GlobalControl.Instance.playerStatistics.Crystals = playerStatistics.Crystals;
            GlobalControl.Instance.SaveData();
        }
        
    }
