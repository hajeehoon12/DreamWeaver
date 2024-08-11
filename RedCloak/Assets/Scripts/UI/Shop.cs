using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shop;
    public List<Button> itemBtn;
    public GameObject goodsList;
    public GameObject parentPosition;
    public GameObject npcObject;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemPrice;

    public NPCInteraction npc;

    public void SetShopGoods()
    {
        ClearShop();

        foreach (ItemData itemdata in npc.shopDataList)
        {
            GameObject goodsInstantiate = Instantiate(goodsList, parentPosition.transform);

            itemName = goodsInstantiate.transform.Find("ItemDisplay/ItemName").GetComponent<TextMeshProUGUI>();
            itemDescription = goodsInstantiate.transform.Find("ItemDisplay/ItemDescription").GetComponent<TextMeshProUGUI>();
            itemPrice = goodsInstantiate.transform.Find("ItemDisplay/ItemPrice").GetComponent<TextMeshProUGUI>();

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

            Button itemBtn = goodsInstantiate.transform.Find("BuyBtn").GetComponent<Button>();

            if (itemBtn != null && npc != null)
            {
                itemBtn.onClick.RemoveAllListeners();
                itemBtn.onClick.AddListener(() => npc.PurchasedItem(itemdata));
                itemBtn.interactable = !npc.HasPurchasedItem(itemdata.name);
            }
        }
    }

    private void ClearShop()
    {
        foreach(Transform child in parentPosition.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
