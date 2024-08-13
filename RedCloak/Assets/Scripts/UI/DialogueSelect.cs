using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class DialogueSelect : MonoBehaviour
{
    public GameObject Shop;
    public NPCInteraction npc;
    public InputActionAsset uiInput;


    private void OnEnable()
    {
        uiInput.Disable();
    }

    private void OnDisable()
    {
        uiInput.Enable();
    }

    public void OpenShop()
    {
        npc = CharacterManager.Instance.Player.controller.currentNpc;
        uiInput.Disable();
        UIManager.Instance.OpenUI(Shop);
    }

    public void CloseShop()
    {
        Shop.SetActive(false);
        uiInput.Enable();
        UIManager.Instance.dialogueUI.StartDialogue(CharacterManager.Instance.Player.controller.currentNpc.dialogueData);
    }

    public void CloseSelectMenu()
    {
        this.gameObject.SetActive(false);
    }
}
