using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSliderVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    public int SliderType;

    void Start()
    {
        if (SliderType == 1)
        {
            slider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        }
        if (SliderType == 2)
        {
            slider.value = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        }
        else
        {
            slider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        }
    }

    public void SetLevelMusic()
    {
        float sliderValue = slider.value;
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }
    public void SetLevelMaster()
    {
        float sliderValue = slider.value;
        mixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }
    public void SetLevelSound()
    {
        float sliderValue = slider.value;
        mixer.SetFloat("SoundVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SoundVolume", sliderValue);
    }
}
