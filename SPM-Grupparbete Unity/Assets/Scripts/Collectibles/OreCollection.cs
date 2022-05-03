using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreCollection : MonoBehaviour
{
    [SerializeField] string oreName = "Blue";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectOre()
    {
        DestoryObject();
    }

    

    private void DestoryObject()
    {
        Destroy(this.gameObject);
    }

}
