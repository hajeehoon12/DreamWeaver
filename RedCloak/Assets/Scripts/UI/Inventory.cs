using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryUI;
    public Transform slotPanel;

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;


    // 선택 버튼으로 장착?

    private int curEquipIndex;

    private void Start()
    {
        
    }

    public bool IsOpen()
    {
        return inventoryUI.activeInHierarchy;
    }

    public void AddItem()
    {
        //플레이어가 가지고 있는 아이템 데이터 가져오기

    }
}
