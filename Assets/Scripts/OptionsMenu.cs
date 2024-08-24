using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public AudioSource audioSource;

    void Start()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        audioSource.volume = masterVolumeSlider.value;
    }

    public void SetMasterVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
}
 
    
