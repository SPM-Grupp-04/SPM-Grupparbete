using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private Material mat;
    public bool hideMat;
    public float dissolveSpeed = 2f;
    
   
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<SkinnedMeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
