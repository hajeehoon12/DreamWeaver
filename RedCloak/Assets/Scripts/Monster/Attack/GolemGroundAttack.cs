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
        if(effect.activeInHierarchy)
            effect.SetActive(false);
        AudioManager.instance.PlayMonsterPitch("GolemGround", 0.1f);
        effect.SetActive(true);
        return false;
    }
}
