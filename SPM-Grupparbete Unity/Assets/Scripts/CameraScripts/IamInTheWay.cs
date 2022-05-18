using System;
using UnityEngine;

namespace Utility.CameraScripts
{
    public class IamInTheWay : MonoBehaviour
    {
        [SerializeField] private GameObject solidBody;
        [SerializeField] private GameObject transperantBody;

        private void Awake()
        {
            ShowSolidBody();
        }

        public void ShowTransperant()
        {
           transperantBody.SetActive(true);
           solidBody.SetActive(false);
        }

        public void ShowSolidBody()
        {
            solidBody.SetActive(true);
            transperantBody.SetActive(false);
        }
    }
}