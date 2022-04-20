
    using UnityEngine;

    public class Cover : MonoBehaviour
    {
        [SerializeField] private Transform[] coverSpors;

        public Transform[] GetCoverSpots()
        {
            return coverSpors;
        }


    }
