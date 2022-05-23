using BehaviorTree;
using UnityEngine;

namespace Utility.DroneAI
{
    public class CheckIfKristalIsInRange : TreeNode
    {
        LayerMask ORE = LayerMask.GetMask("Ore");
        private Transform pos;
        private float fov;

        public CheckIfKristalIsInRange(Transform pos, float fov)
        {
            this.pos = pos;
            this.fov = fov;
        }

        public override NodeState Evaluate()
        {
            // Om Kristaller finns i närheten.
            // gå dit.

            object t = GetData("target");

            if (t == null)
            {
                Collider[] colliders = Physics.OverlapSphere(
                    pos.position, fov, ORE);


                if (colliders.Length > 0)
                {
                    MinableOre g = colliders[0].gameObject.GetComponent<MinableOre>();
                    if (g != null && g.GetRequiredWeaponLevel() >
                        PlayerStatistics.Instance.drillLevel)
                    {
                        Debug.Log(g.GetRequiredWeaponLevel() + " Required WeaponLevel");
                        Debug.Log("Dril level to high");
                        state = NodeState.FAILURE;
                        return state;
                    }
                    
                    parent.parent.SetData("target", colliders[0].transform);


                    state = NodeState.SUCCESS;

                    return state;
                }

                state = NodeState.FAILURE;
                return state;
            }

            return NodeState.FAILURE;
        }
    }
}