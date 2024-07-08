using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject videoSetting;
    [SerializeField] private GameObject audioSetting;

    public void OnClickVideoSetting()
    {
        videoSetting.SetActive(true);
        optionUI.SetActive(false);
    }

    public void OnClickAudioSetting()
    {
        audioSetting.SetActive(true);
        optionUI.SetActive(false);
    }
}
