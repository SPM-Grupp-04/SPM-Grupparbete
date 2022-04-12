using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class EgilSelector : EgilNode

    {
        
        public EgilSelector() : base()
        {
        }

        public EgilSelector(List<EgilNode> children) : base(children)
        {
        }

        public override NodeState Evaluate()
        {
            foreach (EgilNode node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}