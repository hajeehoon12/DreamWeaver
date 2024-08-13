using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject soundSettingUI;

    private void Start()
    {
        SetInitialSlider();
    }

    private void SetInitialSlider()
    {
        float masterVolume;
        float bgmVolume;
        float sfxVolume;

        audioMixer.GetFloat("Master", out masterVolume);
        audioMixer.GetFloat("BGM", out bgmVolume);
        audioMixer.GetFloat("SFX", out sfxVolume);

        volumeSlider.value = Mathf.Pow(10, masterVolume / 20);
        bgmSlider.value = Mathf.Pow(10, bgmVolume / 20);
        sfxSlider.value = Mathf.Pow(10, sfxVolume / 20);
    }

    public void SetMasterVolume(float sliderValue)
    {
        float volume = Mathf.Log10(sliderValue) * 20;

        audioMixer.SetFloat("Master", volume);
    }

    public void SetBGMVolume(float sliderValue)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSFXVolume(float sliderValue)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
    }

    public void OnClickBack()
    {
        UIManager.Instance.OpenUI(optionUI);
    }
}
 