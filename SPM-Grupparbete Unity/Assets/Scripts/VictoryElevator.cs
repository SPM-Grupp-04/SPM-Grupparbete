using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            animator.enabled = false;

        }

        private void OnTriggerEnter(Collider other)
        {
            if(!other.gameObject.CompareTag("Player")) {return;}
            
            if (victorNumber >= 5)
            {
                animator.enabled = true;
             Debug.Log("Hallo");
                Time.timeScale = 0;
                animator.SetTrigger("CloseDoor");
            }
        }

        /*private void OnCollisionEnter(Collision collision)
        {
            if(!collision.gameObject.CompareTag("Player")) {return;}
            
            if (victorNumber >= 5)
            {
             
                Time.timeScale = 0;
                animator.SetBool("doorOpen",true );   
            }
            
        }*/

        public void SendToCreditScene()
        {
            SceneManager.LoadScene(4);
        }

        /*
         * Den ska kolla om du har 5 komponenter kolla om spelaren går in i 
            triggern spela animationen 
         */
    }
}