using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemGroundAttack : MonoBehaviour, IMobAttack
{
    [SerializeField] private MonsterController _controller;
    [SerializeField] private GameObject effect;
    
    private void OnEnable()
    {
        _controller.MobAttack = this;
    }

    private void Start()
    {
        effect.SetActive(false);
    }

    public bool PerformAttack()
    {
        effect.SetActive(true);
        return false;
    }
}