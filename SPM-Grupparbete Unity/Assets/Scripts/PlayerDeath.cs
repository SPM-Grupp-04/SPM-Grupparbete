using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private Rigidbody _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("death", 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void death()
    {
        Destroy(this.gameObject);
    }
}
