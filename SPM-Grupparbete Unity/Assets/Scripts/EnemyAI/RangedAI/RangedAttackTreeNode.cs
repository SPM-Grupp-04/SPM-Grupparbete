﻿using System.Collections.Generic;
using BehaviorTree;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;
using UnityEngine.AI;
using Event = UnityEngine.Event;

namespace Utility.EnemyAI
{
    public class RangedAttackTreeNode : TreeNode
    {
        private NavMeshAgent agent;
        private Vector3 target;
        private Transform firePoint;

        private GameObject throwableObject;
        private Vector3 currentVelocity;
        private float smoothDamp;

        [Header("ThrowSettings")] private float throwCD = 2;
        private float throwUpForce;
        private float throwForce;
        private RangedAI _rangedAI;


        public RangedAttackTreeNode(Transform firePoint,Vector3 target, NavMeshAgent agent,
            GameObject throwabelObject, float throwUpForce, float throwForce, RangedAI rangedAI)
        {
            this.firePoint = firePoint;
            this.agent = agent;
            this.target = target;
            this.throwableObject = throwabelObject;
            smoothDamp = 1f;
            _rangedAI = rangedAI;
            this.throwUpForce = throwUpForce;
            this.throwForce = throwForce;
        }

        public override NodeState Evaluate()
        {
            agent.isStopped = true;
            Transform agentT = agent.transform;

            /*Vector3 direction = target - agentT.position;
            Vector3 currentDirection =
                Vector3.SmoothDamp(agentT.forward, direction, ref currentVelocity, smoothDamp);
            Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
            agentT.rotation = rotation;*/
            
            agentT.LookAt(target);

            if (_rangedAI.timer < 0)
            {
                var shootEventInfo = new EntityShot(firePoint,
                    this.throwableObject, throwUpForce, throwForce);
                EventSystem.current.FireEvent(shootEventInfo);
                _rangedAI.timer = throwCD;
            }


            state = NodeState.RUNNING;
            return state;
        }
    }
}