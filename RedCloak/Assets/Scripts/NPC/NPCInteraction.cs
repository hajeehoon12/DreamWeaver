using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public GameObject dialogueUI;
    public GameObject selectMenu;
    public GameObject shopObj;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public Button upgrade;
    public Button exit;
    public DialogueData dialogueData;
    public LayerMask playerLayer;
    public Shop shop;
    public List<ItemData> shopDataList = new List<ItemData>();
    public GameObject continueKey;

    public bool isPlayerRange = false;
    public bool isDialogue = false;

    public int currentLineIndex = 0;

    private Dictionary<string, bool> itemPurcased;

    private void Awake()
    {
        itemPurcased = new Dictionary<string, bool>();

        foreach(var itemData in shopDataList)
        {
            itemPurcased[itemData.name] = false;
        }

        dialogueUI.SetActive(false);
        selectMenu.SetActive(false);
        shopObj.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerRange = true;
            shop.SetShopGoods();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerRange = false;
        }
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
        if(currentLineIndex < dialogueData.dialogueLines.Count - 1)
        {
            currentLineIndex++;
            continueKey.SetActive(true);
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

    public void PurchasedItem(ItemData itemData)
    {
        if(!HasPurchasedItem(itemData.name))
        {
            CharacterManager.Instance.Player.GetComponent<Player>().stats.ApplyItemEffect(itemData);
            itemPurcased[itemData.name] = true;
            UIManager.Instance.uiBar.UpdateMaxHP(itemData.healthIncrease);
        }

        else
        {
            Debug.Log("already purchased" + itemData.name);
        }
    }

    public bool HasPurchasedItem(string itemName)
    {
        return itemPurcased.ContainsKey(itemName) && itemPurcased[itemName];
    }

    public void OpenShop()
    {
        UIManager.Instance.OpenUI(shopObj);
    }

    private IEnumerator CheckSelectMenu()
    {
        while(isDialogue)
        {
            if(!selectMenu.activeInHierarchy)
            {
                NextDialogue();
                yield break;
            }

            yield return null;
        }
    }
}
