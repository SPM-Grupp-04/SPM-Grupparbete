//author: Simon Canb�ck, sica4801

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryConditionsScript : MonoBehaviour
{
    [SerializeField, Tooltip("")] private Components victoryLayerMask;

    private void Start()
    {
        //remember to set a value
        try
        {
            UnityEngine.Assertions.Assert.AreNotEqual(expected: Components.UNASSIGNED, actual: victoryLayerMask);
        }
        catch (UnityEngine.Assertions.AssertionException)
        {
            Application.Quit();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {
            return;
        }

        if (PlayerStatistics.Instance.componentsCollectedMask >= (int) victoryLayerMask)
        {
          //  Destroy(gameObject);
          gameObject.SetActive(true);
          
        }
    }

    [Flags]
    public enum Components
    {
        UNASSIGNED = 0b_0000_0000,
        COMPONENT_1 = 0b_0000_0001,
        COMPONENT_2 = 0b_0000_0010,
        COMPONENT_3 = 0b_0000_0100,
        COMPONENT_4 = 0b_0000_1000,
        COMPONENT_5 = 0b_0001_0000,
        COMPONENT_6 = 0b_0010_0000,
        COMPONENT_7 = 0b_0100_0000,
        COMPONENT_8 = 0b_1000_0000
    }
}