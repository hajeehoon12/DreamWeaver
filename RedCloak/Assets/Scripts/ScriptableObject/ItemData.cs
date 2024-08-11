using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Goods,
    Equip,
    Antique,
    Health,
    Skill
}

public enum SkillType
{ 
    Skill1,
    Skill2,
    Skill3
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

    [Header("Skill")]
    public SkillType skillType;

    //[Header("Equip")]
    //public GameObject equipPrefab;
}
