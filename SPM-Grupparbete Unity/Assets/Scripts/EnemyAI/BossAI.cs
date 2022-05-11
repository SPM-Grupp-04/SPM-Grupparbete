using System.Collections.Generic;
using BehaviorTree;
using UnityEditor;
using UnityEngine;
using BehaviorTree;
using Tree = BehaviorTree.Tree;

public class BossAI : Tree
    {
        [SerializeField] private Transform partToRotate;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform firepoint;
        [SerializeField] private  float fovAttackRange;
        [SerializeField] private Transform playerTransfrom;
        private float checkForPlayerFOV = 16;

        protected override TreeNode SetUpTree()
        {
          
        
            /*
             * :Needed nodes:
             * Hitta spelarna.
             * Röra sig mot spelarna
             * Slå mot spelarna
             * Spawna Adds
             * Stampa i marken för att få ner stenar som faller
             * Skjuta laser från axeln
             * 
             */
            
            TreeNode root = new Selector(  new List<TreeNode>
                {
                    new Sequence(new List<TreeNode>
                    {
                        new CheckPlayerInAttackRange(transform, checkForPlayerFOV),
                        new BossAttackWithLaser(transform, partToRotate, lineRenderer, firepoint, fovAttackRange),
                        
                    }),
                    
                  
                });
                
            Debug.Log(root);
     
            return root;
        }
    }
