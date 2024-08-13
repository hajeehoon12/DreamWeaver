using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public DialogueData dialogueData;
    public LayerMask playerLayer;
    public List<ItemData> shopDataList = new List<ItemData>();
    public GameObject arrowKey;

    public bool isPlayerRange = false;

    private Dictionary<string, bool> itemPurcased;

    private void Awake()
    {
        itemPurcased = new Dictionary<string, bool>();

        foreach(var itemData in shopDataList)
        {
            itemPurcased[itemData.name] = false;
        }

        arrowKey.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            UIManager.Instance.dialogueUI.dialogueData = dialogueData;
            isPlayerRange = true;
            UIManager.Instance.shop.InitializeShop(this);
            arrowKey.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            UIManager.Instance.dialogueUI.dialogueData = null;
            isPlayerRange = false;
            arrowKey.SetActive(false);
        }
    }

    public void PurchasedItem(ItemData itemData)
    {
        if(!HasPurchasedItem(itemData.name))
        {
            UIManager.Instance.inventory.ApplyItemEffect(itemData);
            //CharacterManager.Instance.Player.GetComponent<Player>().stats.ApplyItemEffect(itemData);
            itemPurcased[itemData.name] = true;
            if(itemData.healthIncrease > 0)
            {
                ItemManager.Instance.healthPartNum += itemData.healthIncrease;
                ItemManager.Instance.synchroniztion();
                UIManager.Instance.uiBar.UpdateMaxHP(itemData.healthIncrease);
            }
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
}
