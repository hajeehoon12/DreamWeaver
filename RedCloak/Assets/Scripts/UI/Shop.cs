using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shop;
    public GameObject goodsList;
    public GameObject parentPosition;
    public GameObject soldOut;
    public GameObject noPrice;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemPrice;
    public Image icon;

    public NPCInteraction npc;

    public void InitializeShop(NPCInteraction npcInteraction)
    {
        npc = npcInteraction;
        SetShopGoods();
    }

    public void SetShopGoods()
    {
        ClearShop();

        noPrice.SetActive(false);

        foreach (ItemData itemdata in NPCInteraction.initialShop)
        {
            GameObject goodsInstantiate = Instantiate(goodsList, parentPosition.transform);

            itemName = goodsInstantiate.transform.Find("ItemDisplay/ItemName").GetComponent<TextMeshProUGUI>();
            itemDescription = goodsInstantiate.transform.Find("ItemDisplay/ItemDescription").GetComponent<TextMeshProUGUI>();
            itemPrice = goodsInstantiate.transform.Find("ItemDisplay/ItemPrice").GetComponent<TextMeshProUGUI>();
            icon = goodsInstantiate.transform.Find("Icon").GetComponent<Image>();
            soldOut = goodsInstantiate.transform.Find("SoldOut").gameObject;

            if (itemName != null)
            {
                itemName.text = itemdata.itemName;
            }

            if (itemDescription != null)
            {
                itemDescription.text = itemdata.description;
            }

            if (itemPrice != null)
            {
                itemPrice.text = itemdata.price.ToString();
            }

            if(icon != null)
            {
                icon.sprite = itemdata.icon;
            }

            Button itemBtn = goodsInstantiate.transform.Find("BuyBtn").GetComponent<Button>();

            if (itemBtn != null && npc != null)
            {
                if (npc.HasPurchasedItem(itemdata.name))
                {
                    itemBtn.interactable = false;
                    if (soldOut != null)
                    {
                        soldOut.SetActive(true);
                    }
                }
                else
                {
                    itemBtn.onClick.RemoveAllListeners();
                    itemBtn.onClick.AddListener(() =>
                    {
                        npc.PurchasedItem(itemdata);
                        if(itemdata.price <= CharacterManager.Instance.Player.stats.playerGold)
                        {
                            CharacterManager.Instance.Player.itemData = itemdata;
                            CharacterManager.Instance.Player.addItem?.Invoke();
                            CharacterManager.Instance.Player.stats.playerGold -= itemdata.price;
                            soldOut.SetActive(true);
                            UIManager.Instance.uiBar.UpdateGold();
                            UpdateUI();
                        }
                        
                        else
                        {
                            noPrice.SetActive(true);
                            StartCoroutine(LowPrice());
                        }
                    });

                    itemBtn.interactable = !npc.HasPurchasedItem(itemdata.name);

                    if (soldOut != null)
                    {
                        soldOut.SetActive(false);
                    }
                }
            }
        }
    }

    IEnumerator LowPrice()
    {
        yield return new WaitForSeconds(2f);
        noPrice.SetActive(false);
    }

    private void ClearShop()
    {
        foreach(Transform child in parentPosition.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnDisable()
    {
        UIManager.Instance.dialogueUI.NextDialogue();
    }

    private void UpdateUI()
    {
        SetShopGoods();
    }
}
