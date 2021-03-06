using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

// AND LOGIC GATE IF ALL CHILDE NODES SUCCEDSS I WILL TO

namespace BehaviorTree
{
    public class Sequence : TreeNode
    {

        public Sequence() : base(){}
        public  Sequence(List<TreeNode> children) :base(children){}
        
        public override NodeState Evaluate()
        {
         bool anyChildRunning = false;
            foreach (TreeNode node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildRunning = true;
                        continue;
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }

            state = anyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }
}