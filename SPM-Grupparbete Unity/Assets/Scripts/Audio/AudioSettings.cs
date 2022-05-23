using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Toggle toggle;

    private bool isMuted = false;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("IsMuted") == false)
        {
            audioMixer.SetFloat("VolumeControl", 1);
            toggle.isOn = false;
            return;
        }

        audioMixer.SetFloat("VolumeControl", PlayerPrefs.GetFloat("IsMuted"));
        toggle.isOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMutedAudio()
    {
        if (isMuted)
        {
            audioMixer.SetFloat("VolumeControl", 1f);
            isMuted = false;
            PlayerPrefs.SetFloat("IsMuted", 0);
            return;
        }

        audioMixer.SetFloat("VolumeControl", -80f);
        isMuted = true;
        PlayerPrefs.SetFloat("IsMuted", 1);


    }
}
