using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Max_OreCollection : MonoBehaviour
{
    [SerializeField] string oreName = "Red";
    private EgilHealth eh;

    // Start is called before the first frame update
    void Start()
    {
        eh = GetComponent<EgilHealth>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectOre()
    {
        DestoryObject();
        eh.GainCrystal();
    }

    

    private void DestoryObject()
    {
        Destroy(this.gameObject);
    }

}
