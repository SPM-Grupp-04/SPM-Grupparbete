using System.Collections.Generic;
using BehaviorTree;
using UnityEditor;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;
using Tree = BehaviorTree.Tree;

public class BossAI : Tree
{
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LineRenderer _lineRendererTwo;
    [SerializeField] private Transform firepoint;
    [SerializeField] private Transform firePointTwo;
    [SerializeField] private float fovAttackRange;
    [SerializeField] private Animator _animator;
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
         * Skjuta laser från ögonen
         * 
         */

        TreeNode root = new Selector(new List<TreeNode>
        {
            // Skjuter spelaren.
            new Sequence(new List<TreeNode>
            {
                new CheckPlayerInAttackRange(agent.transform, checkForPlayerFOV),
                new BossAttackWithLaser(agent.transform, lineRenderer, _lineRendererTwo, firepoint, firePointTwo,
                    fovAttackRange, _animator),
            }),

            // Springer efters seplaren
            new Sequence(new List<TreeNode>
            {
                new CheckPlayerInAttackRange(transform, 20),
                new BossMoveToPlayers(transform,_animator),
            })
            
            // Om spelarna springer för långt ifrån spring tillbaka till din spawn position och börja
            // slöppa stenar från taket.



        });


        return root;
    }
}