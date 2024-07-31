using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject videoSetting;
    [SerializeField] private GameObject audioSetting;

    public void OnClickVideoSetting()
    {
        UIManager.Instance.OpenUI(videoSetting);
    }

    public void OnClickAudioSetting()
    {
        UIManager.Instance.OpenUI(audioSetting);
    }

    public void OnClickBack()
    {
        UIManager.Instance.OpenUI(pauseUI);
    }
}
