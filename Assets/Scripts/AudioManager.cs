using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer; 
    public Slider volumeSlider;   

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("GameVolume", 0.75f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        float clampedVolume = Mathf.Clamp(volume, 0.0001f, 1f);

        audioMixer.SetFloat("MyExposedParam", Mathf.Log10(clampedVolume) * 20);
        
        PlayerPrefs.SetFloat("GameVolume", clampedVolume);
    }
}
