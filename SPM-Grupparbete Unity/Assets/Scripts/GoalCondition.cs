using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalCondition : MonoBehaviour
{
    [SerializeField] private int goalCondition;
    private void OnCollisionEnter(Collision collision)
    {
        if (PlayerStatistics.Instance.componentsCollectedMask >= goalCondition)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
