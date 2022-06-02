using System;
using UnityEngine;

namespace Utility
{
    public class VictoryElevator : MonoBehaviour
    {
        private int victorNumber;
        private Animator animator;
        private void Start()
        {
            victorNumber = GlobalControl.Instance.playerStatistics.componentsCollectedNumber;
            animator = GetComponent<Animator>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            
            Time.timeScale = 0;
            animator.SetBool("doorOpen",victorNumber >= 5 );
            
        }

        /*
         * Den ska kolla om du har 5 komponenter kolla om spelaren går in i 
            triggern spela animationen 
         */
    }
}