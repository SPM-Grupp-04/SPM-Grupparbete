using UnityEngine;
using BehaviorTree;


public class EgilAttackEnemy : EgilNode
{
    // This scrip shoud attack the enemy
    private Transform lastTarget;
    private Egil_EnemyTakeDamage enemyTakeDamage;


    private float attackTime = 1f;
    private float attackCounter = 0f;

    public EgilAttackEnemy(Transform transform)
    {
    }

    public override NodeState Evaluate()
    {
        
        Transform target = (Transform) GetData("Target");
        if (target != lastTarget)
        {
            enemyTakeDamage = target.GetComponent<Egil_EnemyTakeDamage>();
        }

        attackCounter += Time.deltaTime;

        if (attackCounter >= attackTime)
        {
            bool enemyIsDead = enemyTakeDamage.getIsDead();
            if (enemyIsDead)
            {
                enemyTakeDamage.killEnemy();
                ClearData("target");
            }
            else
            {
                enemyTakeDamage.TakeDamage();
                attackCounter = 0;
            }
        }


        // Implement attackScript!.

        Debug.Log("Attacking Enemy");

        state = NodeState.RUNNING;
        return state;
    }
}