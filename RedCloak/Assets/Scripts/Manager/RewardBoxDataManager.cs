using System;
using UnityEngine;

public class RewardBoxDataManager : MonoBehaviour
{
    [SerializeField] private RewardChest[] box;
    public static RewardBoxDataArray array { get; private set; }
    
    private void Awake()
    {
        if (array == null)
            array = SaveLoad.Load<RewardBoxDataArray>("RewardBoxData");

        for (int i = 0; i < box.Length; i++)
        {
            box[i].index = int.Parse(array.data[i].rcode.Substring(array.data[i].rcode.Length - 5)) - 1;
            box[i].isOpen = array.data[i].isOpen;
        }
    }
    
    public static void ChangeOpenStat(int index)
    {
        array.data[index].isOpen = true;
    }
}

[Serializable]
public class RewardBoxDataArray
{
    public RewardBoxData[] data;
}

[Serializable]
public struct RewardBoxData
{
    public string rcode;
    public bool isOpen;
}