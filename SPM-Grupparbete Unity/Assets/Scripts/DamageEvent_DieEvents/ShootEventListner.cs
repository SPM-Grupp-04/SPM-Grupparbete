using EgilEventSystem;
using UnityEngine;

namespace EgilScripts.DieEvents
{
    public class ShootEventListner : MonoBehaviour
    {
        private void Start()
        {
            EventSystem.current.RegisterListner<ShootEventInfo>(OnShot);
        }

        void OnShot(ShootEventInfo shootEventInfo)
        {
            // Instancera Granaten / Skottet som fienden ska kasta från sig.
            // Vart ska granaten kastas och vart ifråN?
            Debug.Log("Shooting the player");
        }
    }
}