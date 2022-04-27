using System.Net.NetworkInformation;
using EgilEventSystem;
using Unity.Mathematics;
using UnityEngine;

namespace EgilScripts.DieEvents
{
    public class ShootEventListner : MonoBehaviour
    {
        private bool readyTothrow = true;
       

        private void Start()
        {
            EventSystem.current.RegisterListner<ShootEventInfo>(OnShot);
        }

        void OnShot(ShootEventInfo shootEventInfo)
        {
            if (readyTothrow)
            {
                Throw(shootEventInfo);
            }
        }

        private void Throw(ShootEventInfo shootEventInfo)
        {
            readyTothrow = false;
            GameObject projectile =
                Instantiate(shootEventInfo.throwableObject, shootEventInfo.ogPos.transform.position,
                    quaternion.identity);

            Rigidbody projetileRb = projectile.GetComponent<Rigidbody>();

        
            Vector3 forceToAdd = shootEventInfo.throwForce * shootEventInfo.ogPos.transform.forward +
                                 shootEventInfo.ogPos.transform.up * shootEventInfo.throwUpwardForce;

            projetileRb.AddForce(forceToAdd, ForceMode.Impulse);

            Invoke(nameof(ResetThrow), shootEventInfo.throwCooldown);
        }

        private void ResetThrow()
        {
            readyTothrow = true;
        }
    }
}