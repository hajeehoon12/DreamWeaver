using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public ItemPopup itemPopup;
    public ChanageStage changeStage;
    public MiniMap miniMap;
    public UIBar uiBar;
    public Pause pause;
    public SkillUI skillUI;
    public Inventory inventory;
    public DialogueUI dialogueUI;
    public Shop shop;

    public GameObject currentUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenUI(GameObject uiElement)
    {
        if(currentUI != null)
        {
            currentUI.SetActive(false);
        }

        uiElement.SetActive(true);
        currentUI = uiElement;
    }

 

    public void ChangeMenu(GameObject currentUI, GameObject targetUI)
    {
        if(currentUI != null)
        {
            currentUI.SetActive(false);
        }

        targetUI.SetActive(true);
        currentUI = targetUI;
    }

    public void CloseCurrentUI()
    {
        if(currentUI != null)
        {
            currentUI.SetActive(false);
            Time.timeScale = 1f;
            pause.EnablePlayerInput();
            currentUI = null;
        }
    }

    public bool IsUIOpen(GameObject uiElement)
    {
        return currentUI == uiElement;
    }

    public bool IsAnyUIOpen()
    {
        return currentUI != null;
    }
}
