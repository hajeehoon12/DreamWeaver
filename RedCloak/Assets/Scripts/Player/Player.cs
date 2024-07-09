using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public PlayerController controller;
   
    public PlayerBattle battle;
    public Pet pet;
    public PlayerStat stats;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        //stats = GetComponent<PlayerStat>();
        battle = GetComponent<PlayerBattle>();


    }


    private void Start()
    {
        pet = GameObject.Find("PetLight").GetComponent<Pet>();
        StageChangeManager.Instance.SaveScene();
    }

}