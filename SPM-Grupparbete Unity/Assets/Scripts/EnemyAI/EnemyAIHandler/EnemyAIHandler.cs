using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAIHandler : MonoBehaviour
{
    private static EnemyAIHandler instance;

    public static EnemyAIHandler Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    public List<BaseClassEnemyAI> units = new List<BaseClassEnemyAI>();

    
    
}
