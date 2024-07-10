using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushAttack : MonoBehaviour, IMobAttack
{
    [SerializeField] private MonsterController _controller;
    
    private Vector2 detectionSize = new Vector2(4, 2);

    private void Start()
    {
        _controller.MobAttack = this;
    }

    public bool PerformAttack()
    {
        AudioManager.instance.PlaySFX("BushmonAttack", 0.2f);
        RaycastHit2D hit = Physics2D.BoxCast((Vector2)transform.position, detectionSize, 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Player"));
        
        if (hit)
        {
            hit.collider.GetComponent<PlayerBattle>().ChangeHealth(-1, transform.position);
        }

        return hit;
    }
}
