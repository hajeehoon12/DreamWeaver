using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryUI;
    public Transform slotPanel;

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public Image selectImage;

    private int selectedItemIndex;
    private int curEquipIndex;

    private void Start()
    {
        CharacterManager.Instance.Player.addItem += AddItem;
        slots = new ItemSlot[slotPanel.childCount];

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }
    }

    public bool IsOpen()
    {
        return inventoryUI.activeInHierarchy;
    }

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;
        ItemSlot emptySlot = GetEmptySlot();
        if(data != null && !data.canBuy)
        {
            UIManager.Instance.itemPopup.PopupGetItem();
        }

        if (emptySlot != null )
        {
            emptySlot.item = data;
            UpdateInventory();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        if(data == null)
        {
            Debug.Log("AddItem data is null");
            return;
        }
    }

    ItemSlot GetEmptySlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }

        return null;
    }

    public void UpdateInventory()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].SetItem();
            }

            else
            {
                slots[i].Clear();
            }
        }
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        itemName.text = selectedItem.item.itemName;
        itemDescription.text = selectedItem.item.description;
        selectImage.sprite = selectedItem.item.icon;
    }

    private void ClearSelectItem()
    {
        selectedItem = null;

        itemName.text = string.Empty;
        itemDescription.text = string.Empty;
    }
}
