using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;


    private void Start()
    {
        SetInitialSlider();
    }

    private void SetInitialSlider()
    {
        float bgmVolume;
        float sfxVolume;

        audioMixer.GetFloat("BGM", out bgmVolume);
        audioMixer.GetFloat("SFX", out sfxVolume);

        bgmSlider.value = Mathf.Pow(10, bgmVolume / 20);
        sfxSlider.value = Mathf.Pow(10, sfxVolume / 20);
    }

    public void SetMasterVolume(float sliderValue)
    {
        float volume = Mathf.Log10(sliderValue) * 20;

        audioMixer.SetFloat("BGM", volume);
        audioMixer.SetFloat("SFX", volume);
    }

    public void SetBGMVolume(float sliderValue)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSFXVolume(float sliderValue)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
    }
}
 