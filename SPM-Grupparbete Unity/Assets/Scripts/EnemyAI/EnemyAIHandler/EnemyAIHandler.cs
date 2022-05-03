using System;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.Pool;
using Tree = BehaviorTree.Tree;


public class EnemyAIHandler : MonoBehaviour
{
    // Ref till spelarna:
    //[SerializeField] private GameObject targetOne;
    //[SerializeField] private GameObject targetTwo;
    
    public List<BaseClassEnemyAI> units = new List<BaseClassEnemyAI>();

    private void Start()
    {
       
    }

    private void Update()
    {
        
        foreach (BaseClassEnemyAI enemyAI in units)
        {
            if (enemyAI.gameObject.activeInHierarchy)
            {
                enemyAI.m_TopTreeNode.Evaluate();
                
            }
        }
    }
}
