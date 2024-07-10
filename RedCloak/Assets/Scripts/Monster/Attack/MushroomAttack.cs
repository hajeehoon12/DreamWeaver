using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAttack : MonoBehaviour, IMobAttack
{
    [SerializeField] private MonsterController _controller;
    [SerializeField] private Vector2 attackRayPos;
    [SerializeField] private float attackRange;

    private void OnEnable()
    {
        _controller.MobAttack = this;
    }

    public bool PerformAttack()
    {
        AudioManager.instance.PlaySFX("MushroomAttack", 0.2f);
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + attackRayPos, transform.right, attackRange,
            1 << LayerMask.NameToLayer("Player"));
        
        if (hit)
        {
            hit.collider.GetComponent<PlayerBattle>().ChangeHealth(-1, transform.position);
        }

        return hit;
    }
}
