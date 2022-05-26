using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{

    [SerializeField] private AudioMixer audioMixer;

   [SerializeField] private Slider audioSlider;
    
    void Start()
    {
        //audioSlider = gameObject.GetComponent<Slider>();
        
     

        if (PlayerPrefs.HasKey("Volume"))
        {
            audioMixer.SetFloat("VolumeControl", PlayerPrefs.GetFloat("Volume"));
            audioSlider.value = PlayerPrefs.GetFloat("Volume");
        }
        else
        {
            Debug.Log(audioSlider.value);
            audioSlider.value = 1;
        }
    }
    
    public void ChangeVolume(float sliderValue)
    {
        audioMixer.SetFloat("VolumeControl", sliderValue);
        PlayerPrefs.SetFloat("Volume", sliderValue);
    }
}
