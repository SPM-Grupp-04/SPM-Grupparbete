using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EgilCollisionHandeler : MonoBehaviour
{
    [SerializeField] int level;

    private EgilHealth eh;
    private EgilPlayerStatistics localEgilPlayerDataTest = EgilPlayerStatistics.Instance;

    private float cooldownTime = 2f; // Varför måste den vara i hela sekunder.
    private float timeRemaning;
    private void Start()
    {
        eh = GetComponent<EgilHealth>();
        timeRemaning = cooldownTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("Goal"))
        {
            int current = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(level);
        }
        if (collision.transform.tag.Equals("Currency"))
        {
            collision.gameObject.SendMessage("CollectOre");

            localEgilPlayerDataTest.Crystals++;
            SavePlayer();

        }
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.transform.tag.Equals("Enemy") && timeRemaning < 0  )
        {
            Debug.Log("gettingHIt");
            
            eh.Takedamage();

        }

        if (timeRemaning < 0)
        {
            timeRemaning = cooldownTime;
        }
        timeRemaning -= Time.deltaTime;

    }

    private void SavePlayer()
    {
        EgilGlobalControl.Instance.SavedData = localEgilPlayerDataTest;
    }


}