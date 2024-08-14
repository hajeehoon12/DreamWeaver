using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField]
    public PlayerController controller;
   
    public PlayerBattle battle;
    public Pet pet;
    public PlayerStat stats;
    public ItemData itemData;
    public System.Action addItem;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        //stats = GetComponent<PlayerStat>();
        battle = GetComponent<PlayerBattle>();
        


    }


    private void Start()
    {
        CharacterManager.Instance.isLoadScene = true;
        pet = GameObject.Find("PetLight").GetComponent<Pet>();

        if (!CharacterManager.Instance.haveSave)
        {
            StageChangeManager.Instance.SaveScene();
            CharacterManager.Instance.SaveInfo();
        }
    }

}