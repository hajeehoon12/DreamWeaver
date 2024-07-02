using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



[Serializable]

public class PlayerStat : MonoBehaviour
{
    public float playerSpeed = 10;

    public float jumpPower = 15;

    public float attackDamage = 5;

    public float playerHP = 4; // temp

    public float playerMaxHP = 4; // temp

    public float playerMP = 50; // temp

    public float playerMaxMP = 50; // temp

    public float playerGold = 0f;

    public void AddGold(float gold)
    {
        DOTween.To(() => playerGold, x => playerGold = x, playerGold + gold, 0.3f);
    }

}
