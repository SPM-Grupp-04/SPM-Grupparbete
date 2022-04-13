using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Max_DeleteRocks : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Invoke("DestoryObject", 5);
    }

    private void DestoryObject()
    {
        Destroy(this.gameObject);

    }
}
