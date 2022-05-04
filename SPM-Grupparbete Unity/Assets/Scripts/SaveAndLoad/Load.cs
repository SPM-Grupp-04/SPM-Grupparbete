
    using System;
    using System.IO;
    using UnityEngine;

    public class Load : MonoBehaviour
    {
        private void Start()
        {
            if (!Directory.Exists("Saves"))
            {
                return;
            }
            GlobalControl.Instance.LoadData();
            GlobalControl.Instance.IsSceneBeingLoaded = true;
        }
    }
