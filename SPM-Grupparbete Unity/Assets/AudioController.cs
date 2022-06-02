using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioController : MonoBehaviour
{

    [SerializeField] AudioClip audioOne, audioTwo, audioThree;
    private AudioClip[] audioClips = new AudioClip[3];

    private AudioSource source;
    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();

        audioClips[0] = audioOne;
        audioClips[1] = audioTwo;
        audioClips[2] = audioThree;
        
        
        
        
        source.clip = audioClips[Random.Range(0, audioClips.Length)];
        //source.pitch = Random.Range(0.2f, 1f);
       // source.time = Random.Range(0f, source.clip.length - 1.5f);

    }

    
}
