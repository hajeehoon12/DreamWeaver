using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialogueUI;
    public GameObject selectMenu;
    public GameObject shopObj;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public DialogueData dialogueData;
    public Button shopBtn;
    public Button exit;

    public GameObject continueKey;

    public int currentLineIndex = 0;

    public bool isDialogue = false;

    private void Awake()
    {
        dialogueUI.SetActive(false);
        selectMenu.SetActive(false);
        shopObj.SetActive(false);
    }

    public void StartDialogue(DialogueData dialogue)
    {
        continueKey.SetActive(false);
        isDialogue = true;
        UIManager.Instance.OpenUI(dialogueUI);
        UIManager.Instance.pause.DisablePlayerInput();
        dialogueData = dialogue;
        currentLineIndex = 0;
        OpenSelectMenu();
        CurrnetLine();
    }

    public void OpenSelectMenu()
    {
        selectMenu.SetActive(true);
        StartCoroutine(CheckSelectMenu());
    }

    public void NextDialogue()
    {
        if (dialogueData == null || dialogueData.dialogueLines == null)
        {
            return;
        }

        if (currentLineIndex < dialogueData.dialogueLines.Count - 1)
        {
            currentLineIndex++;
            if (continueKey != null)
            {
                continueKey.SetActive(true);

            }
            CurrnetLine();
        }

        else
        {
            EndDialogue();
        }
    }

    private void CurrnetLine()
    {
        DialogueData.DialogueLine line = dialogueData.dialogueLines[currentLineIndex];
        speakerText.text = line.name;
        dialogueText.text = line.dialogueText;
    }

    public void EndDialogue()
    {
        UIManager.Instance.CloseCurrentUI();
        UIManager.Instance.pause.EnablePlayerInput();
    }

    private IEnumerator CheckSelectMenu()
    {
        while (isDialogue)
        {
            if (!selectMenu.activeInHierarchy)
            {
                NextDialogue();
                yield break;
            }

            yield return null;
        }
    }
    public void OpenShop()
    {
        UIManager.Instance.OpenUI(shopObj);
    }
}
