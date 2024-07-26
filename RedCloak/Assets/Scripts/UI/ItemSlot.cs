using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;
    public Inventory inventory;
    public Button button;
    public Image icon;
    public TextMeshProUGUI text;
    // 장착 테두리 모양 고민해보기
    private Outline outline;

    public int index;
    public bool equipped;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void SetItem()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
    }

    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        inventory.SelectItem(index);
    }
}
