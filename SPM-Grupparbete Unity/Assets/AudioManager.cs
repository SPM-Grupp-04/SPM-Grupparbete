using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] backgroundMusic;
    [SerializeField] private AudioClip[] combatMusic;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource sourceOne, sourceTwo;
    
    
    private int index;

    private bool inCombat = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("IsMuted"));
        if (PlayerPrefs.GetFloat("IsMuted") == 1)
        {
            audioMixer.SetFloat("VolumeControl", -80f);
        }
        else
        {
            audioMixer.SetFloat("VolumeControl", 1f);
        }
        sourceOne.clip = backgroundMusic[0];
        sourceTwo.clip = combatMusic[0];
        sourceTwo.Play();
        sourceOne.Play();
        index = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!sourceOne.isPlaying && inCombat == false)
        {
            index++;
            sourceOne.clip = backgroundMusic[index];
            sourceOne.Play();
            
        }

        if (index > backgroundMusic.Length - 1)
        {
            index = 0;
        }
        
    }

    public void CombatMusic()
    {
        inCombat = true;
        sourceTwo.clip = combatMusic[index];
        audioMixer.FindSnapshot("CombatMusic").TransitionTo(1f);
        Debug.Log("Här");
    }

    public bool InCombat()
    {
        return inCombat;
    }
}
