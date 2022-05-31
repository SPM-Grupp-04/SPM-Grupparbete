using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionHandler : MonoBehaviour
{
    private PlayerStatistics m_LocalPlayerDataTest = PlayerStatistics.Instance;
    [SerializeField] private AudioManager audioManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("Currency"))
        {
            collision.gameObject.GetComponent<OreCollection>().CollectOre();
            //audioManager.PlayCrystalPickUpSound();
            switch (collision.gameObject.GetComponent<OreCollection>().GetName())
            {
                case "Blue":
                    m_LocalPlayerDataTest.BlueCrystals++;
                    break;
                case "Red":
                    m_LocalPlayerDataTest.RedCrystals++;
                    break;
                case "Green":
                    m_LocalPlayerDataTest.GreenCrystals++;
                    break;

                default:
                    break;

            }
        }

        GlobalControl.Instance.playerStatistics.BlueCrystals = m_LocalPlayerDataTest.BlueCrystals;
        GlobalControl.Instance.playerStatistics.RedCrystals = m_LocalPlayerDataTest.RedCrystals;
        GlobalControl.Instance.playerStatistics.GreenCrystals = m_LocalPlayerDataTest.GreenCrystals;
    }

    
}