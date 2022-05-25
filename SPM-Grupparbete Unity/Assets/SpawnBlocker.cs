using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlocker : MonoBehaviour
{
    private Dictionary<String, GameObject> players = new Dictionary<string, GameObject>();
    [SerializeField] private GameObject rocksToEnable;

    private void Start()
    {
        rocksToEnable.SetActive(false);
    }

    private void Update()
    {
        if (players.ContainsKey("Player1") && players.ContainsKey("Player2"))
        {
            rocksToEnable.SetActive(true);
           // this.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(!other.CompareTag("Player")){return;}

        if (other.gameObject.name.Equals("Player1") && !players.ContainsKey("Player1"))
        {
            players.Add("Player1", other.gameObject);
        }
        
        if (other.gameObject.name.Equals("Player2") && !players.ContainsKey("Player2"))
        {
            players.Add("Player2", other.gameObject);
        }
        
    }


    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Player")){return;}

        if (other.gameObject.name.Equals("Player1") && players.ContainsKey("Player1"))
        {
            players.Remove("Player1");
        }
        
        if (other.gameObject.name.Equals("Player2") && players.ContainsKey("Player2"))
        {
            players.Remove("Player2");
        }

    }
}
