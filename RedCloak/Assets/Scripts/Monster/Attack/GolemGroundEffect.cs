using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemGroundEffect : MonoBehaviour
{
    [SerializeField] private MonsterController _controller;
    [SerializeField] private ParticleSystem ps;


    void Start()
    {
        ps.trigger.SetCollider(0, CharacterManager.Instance.Player.transform);
    }

    private void OnParticleTrigger()
    {
        _controller.CallAttackEvent();
    }
}
