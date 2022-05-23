using System.Net.NetworkInformation;
using EgilEventSystem;
using Unity.Mathematics;
using UnityEngine;

namespace EgilScripts.DieEvents
{
    public class EntityShotListener : MonoBehaviour
    {
        private EventSystem.EventListener shotListener;

        private void Start()
        {
            shotListener = EventSystem.current.RegisterListener<EntityShot>(OnShot);
        }

        void OnShot(EntityShot shootEventInfo)
        {
            TossObject(shootEventInfo);
        }

        private void TossObject(EntityShot shootEventInfo)
        {
            GameObject projectile = Instantiate(shootEventInfo.throwableObject, shootEventInfo.ogPos.transform.position, quaternion.identity);

            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            Vector3 forceToAdd = shootEventInfo.throwForce * shootEventInfo.ogPos.transform.forward
                + shootEventInfo.ogPos.transform.up * shootEventInfo.throwUpwardForce;

            projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
        }

        private void OnDestroy()
        {
            EventSystem.current.UnregisterListener<EntityShot>(shotListener);
        }
    }
}