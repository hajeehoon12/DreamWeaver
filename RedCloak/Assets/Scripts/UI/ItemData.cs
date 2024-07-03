using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Goods,
    Equip
}
[CreateAssetMenu(fileName = " Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    public string description;
    public ItemType type;
    //아이콘
    //오브젝트

    [Header("Equip")]
    public GameObject equipPrefab;
}
