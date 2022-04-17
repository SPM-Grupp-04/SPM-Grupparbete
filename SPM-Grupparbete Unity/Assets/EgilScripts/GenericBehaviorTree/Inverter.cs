namespace BehaviorTree
{
    public class Inverter : EgilNode
    {
        
        public override NodeState Evaluate()
        {
            foreach (EgilNode node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.SUCCESS:
                        state = NodeState.FAILURE;
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