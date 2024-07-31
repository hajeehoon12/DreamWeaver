using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "New Skill")]
public class SkillData : ScriptableObject
{
    [Header("Info")]
    public string skillName;
    public int usageMana;
    public Sprite icon;
}
