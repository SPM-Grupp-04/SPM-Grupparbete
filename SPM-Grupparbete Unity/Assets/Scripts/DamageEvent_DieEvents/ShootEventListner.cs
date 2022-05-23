using System.Net.NetworkInformation;
using EgilEventSystem;
using Unity.Mathematics;
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
            Throw(shootEventInfo);
        }

        private void Throw(ShootEventInfo shootEventInfo)
        {
           
            GameObject projectile = Instantiate(shootEventInfo.throwableObject, shootEventInfo.ogPos.transform.position,
                quaternion.identity);

            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();


            Vector3 forceToAdd = shootEventInfo.throwForce * shootEventInfo.ogPos.transform.forward +
                                 shootEventInfo.ogPos.transform.up * shootEventInfo.throwUpwardForce;


            projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
        }
    }
}