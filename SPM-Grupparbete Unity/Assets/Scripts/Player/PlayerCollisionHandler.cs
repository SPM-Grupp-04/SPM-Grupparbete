using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionHandler : MonoBehaviour
{
    private PlayerStatistics m_LocalPlayerDataTest = PlayerStatistics.Instance;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("Currency"))
        {
            collision.gameObject.GetComponent<OreCollection>().CollectOre();
            switch (collision.gameObject.GetComponent<OreCollection>().GetName())
            {
                case "Blue":
                    m_LocalPlayerDataTest.BlueCrystals++;
                    break;
                case "Red":
                    m_LocalPlayerDataTest.RedCrystals++;
                    break;
                case "Colour":
                    break;

                default:
                    Debug.Log("No name");
                    break;

            }
        }

        GlobalControl.Instance.playerStatistics = m_LocalPlayerDataTest;
    }

    
}