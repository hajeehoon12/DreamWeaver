using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Searcher;
using UnityEngine;

public class FloorSlimeAttack : MonoBehaviour, IMobAttack
{
    [SerializeField] private Vector2 attackScope;
    [SerializeField] private MonsterController _controller;

    private void OnEnable()
    {
        _controller.MobAttack = this;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,attackScope);
    }

    public bool PerformAttack()
    {
        return Physics2D.BoxCast(transform.position, attackScope, 0f, Vector2.zero, 0,
            1 << LayerMask.NameToLayer(Define.PLAYER));
    }
}
