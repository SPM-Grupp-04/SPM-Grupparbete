using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Max_OreCollection : MonoBehaviour
{
    [SerializeField] string oreName = "Red";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int CollectOre()
    {
        DestoryObject();
        return 1;
    }

    

    private void DestoryObject()
    {
        Destroy(this.gameObject);
    }

}
