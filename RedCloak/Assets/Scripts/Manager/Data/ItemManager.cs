using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager _instance;

    public static ItemManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("ItemManager").AddComponent<ItemManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }



    public bool airShoes = false;
    //public bool dash = false;

    public int healthPartNum = 0;


    public void synchroniztion()
    {
        //Debug.Log("Make sync");
        PlayerController controller;
        Player player;
        player = CharacterManager.Instance.Player;
        controller = player.controller;

        //controller.canDoubleJump = airShoes;
        //controller.canDash = dash;
        //Debug.Log(healthPartNum);
        player.stats.playerMaxHP= CharacterManager.Instance.playerInitHealth + healthPartNum;
        player.stats.playerHP = CharacterManager.Instance.playerInitHealth + healthPartNum ;
        

    }

}
