using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeSlimeAttack : MonoBehaviour, IMobAttack
{
    [SerializeField] private BehaviorTree _behavior;
    [SerializeField] private MonsterController _controller;
    [SerializeField] private Animator _animator;

    [SerializeField] private GreenSlimeAttack jump;
    [SerializeField] private MushroomAttack meele;

    [SerializeField] private float meeleRange;

    private IMobAttack attack;
    public bool isMeele { get; set; }

    private void OnEnable()
    {
        _behavior.SetVariable("MeeleRange",(SharedFloat)meeleRange);
        _controller.MobAttack = this;
    }

    public bool PerformAttack()
    {
        Debug.Log(isMeele);
        if (isMeele)
        {
            attack = meele;
        }
        else
        {
            attack = jump;
        }
        
        bool result = attack.PerformAttack();
        
        return result;
    }
}
