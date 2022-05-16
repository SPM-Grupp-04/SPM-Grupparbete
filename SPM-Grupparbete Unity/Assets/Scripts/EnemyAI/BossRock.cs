using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;
using UnityEngine.Assertions;

namespace Utility.EnemyAI
{
    public class BossRock : MonoBehaviour
    {
            [SerializeField] private int damage = 10;
          
            [SerializeField, Tooltip("Mark any layers that the rock is supposed to interact with -- most likely Player and Terrain or similar.")] private LayerMask layerMask;
            
            // Start is called before the first frame update
            
        
          
            void OnCollisionEnter(Collision collision)
            {
                if (Utility.LayerMaskExtensions.IsInLayerMask(collision.gameObject, layerMask))
                {
                   
                    var damageEvent = new DealDamageEventInfo(collision.gameObject, damage);
                    EventSystem.current.FireEvent(damageEvent);
                    
                    Destroy(gameObject);
                }
            }
    }
}