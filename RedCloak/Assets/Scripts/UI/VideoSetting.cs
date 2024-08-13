using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class VideoSetting : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentResolution;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject videoSettingUI;
    [SerializeField] private Toggle vSyncToggle;
    [SerializeField] private Toggle fullScreenToggle;
    public RectTransform dialoguePanel;

    private int screenWidth;
    private int screenHeight;
    private int index = 0;

    private void Start()
    {
        CurrentResolution();
        InitVSync();
        InitFullScreen();
    }

    private void InitVSync()
    {
        vSyncToggle.isOn = QualitySettings.vSyncCount > 0;
    }

    private void InitFullScreen()
    {
        fullScreenToggle.isOn = Screen.fullScreen;
    }

    public void VsyncOption(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;
    }

    public void FullScreen(bool isOn)
    {
        Screen.fullScreen = isOn;
    }


    private struct ResolutionOption
    {
        public int width;
        public int height;

        public ResolutionOption(int w, int h)
        {
            width = w;
            height = h;
        }

        public override string ToString()
        {
            return $"{width} x {height}";
        }
    }

    private List<ResolutionOption> resolutions = new List<ResolutionOption>
    {
        new ResolutionOption(1920, 1080),
        new ResolutionOption(1280, 720),
        new ResolutionOption(800, 600)
    };

    private void CurrentResolution()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        currentResolution.text = $"{screenWidth} x {screenHeight}";
    }

    public void NextResolution()
    {
        index++;
        if(index >= resolutions.Count)
        {
            index = 0;
        }
        UpdateResolutionText();
        SetResolution();
        AdjustDialoguePanel();
        /*
        var gvWndType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
        var selectedSizeIndexProp = gvWndType.GetProperty("selectedSizeIndex",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var gvWnd = EditorWindow.GetWindow(gvWndType);
        selectedSizeIndexProp.SetValue(gvWnd, index, null);*/
    }

    public void PreviousResolution()
    {
        index--;
        if(index < 0)
        {
            index = resolutions.Count - 1;
        }
        UpdateResolutionText();
        SetResolution();
        AdjustDialoguePanel();
    }

    private void UpdateResolutionText()
    {
        currentResolution.text = resolutions[index].ToString();
    }

    private void SetResolution()
    {
        ResolutionOption resolutionOption = resolutions[index];
        Screen.SetResolution(resolutionOption.width, resolutionOption.height, fullScreenToggle.isOn);
    }

    public void OnClickBack()
    {
        UIManager.Instance.OpenUI(optionUI);
    }

    private void AdjustDialoguePanel()
    {
        dialoguePanel.sizeDelta = new Vector2(screenWidth * 1f, dialoguePanel.sizeDelta.y);
    }
}
