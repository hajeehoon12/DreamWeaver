using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Goods,
    Equip,
    antique
}
[CreateAssetMenu(fileName = " Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    public string description;
    public ItemType type;
    public Sprite icon;
    //아이콘
    //오브젝트

    [Header("Drop")]
    public GameObject dropPrefab;

    //[Header("Equip")]
    //public GameObject equipPrefab;
}
