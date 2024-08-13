using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject miniMap;

    private void Start()
    {
        pauseUI.SetActive(false);
        inventory.SetActive(false);
    }

    public void OnPauseUI(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.phase == InputActionPhase.Started)
        {
            if(UIManager.Instance.currentUI != null && UIManager.Instance.currentUI != pauseUI)
            {
                UIManager.Instance.CloseCurrentUI();
                return;
            }

            if(UIManager.Instance.IsUIOpen(pauseUI))
            {
                UIManager.Instance.CloseCurrentUI();
            }

            else
            {
                UIManager.Instance.OpenUI(pauseUI);
            }
        }
    }

    public void OnInventory(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.phase == InputActionPhase.Started)
        {
            if(UIManager.Instance.IsUIOpen(inventory))
            {
                UIManager.Instance.CloseCurrentUI();
            }

            else
            {
                UIManager.Instance.OpenUI(inventory);
            }
        }
    }

    public void ClosePopup(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.phase == InputActionPhase.Started && UIManager.Instance.itemPopup.isContinue == true)
        {
            UIManager.Instance.itemPopup.ClosePopup();
        }
    }

    public void OnMiniMap(InputAction.CallbackContext callbackContext)
    {
        if(miniMap.activeInHierarchy)
        {
            miniMap.SetActive(false);
        }

        else
        {
            miniMap.SetActive(true);
        }
    }

    public void NPCInteraction(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started && CharacterManager.Instance.Player.controller.currentNpc != null)
        {
            UIManager.Instance.dialogueUI.StartDialogue(CharacterManager.Instance.Player.controller.currentNpc.dialogueData);
        }
    }

    public void ContinueDialogue(InputAction.CallbackContext callbackContext)
    {
        if(CharacterManager.Instance.Player.controller.currentNpc != null)
        {
            if (callbackContext.phase == InputActionPhase.Started && UIManager.Instance.dialogueUI.isDialogue)
            {
                UIManager.Instance.dialogueUI.NextDialogue();
            }
        }
        
    }
}
