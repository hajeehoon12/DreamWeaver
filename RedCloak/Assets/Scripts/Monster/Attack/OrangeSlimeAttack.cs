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
    public bool isMeele;

    private void Start()
    {
        _behavior.SetVariable("MeeleRange",(SharedFloat)meeleRange);
        _controller.MobAttack = this;
    }

    public bool PerformAttack()
    {
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(0,5), new Vector3(30,10));
    }
}
