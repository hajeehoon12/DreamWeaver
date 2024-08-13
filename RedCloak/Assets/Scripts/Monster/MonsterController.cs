using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public event Action AttackEvent;
    private WaitForSeconds invincible;
    public IMobAttack MobAttack;

    private bool collisionOff
    {
        get
        {
            return Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer(Define.PLAYER),
                LayerMask.NameToLayer(Define.ENEMY));
        }
    }

    public bool OnAttack { get; set; }= false;

    private void Start()
    {
        AttackEvent += playerAttack;
        invincible = new WaitForSeconds(CharacterManager.Instance.Player.battle.healthChangeDelay);
    }

    public void Attack()
    {
        if (MobAttack != null)
        {
            if (MobAttack.PerformAttack())
            {
                CallAttackEvent();
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!collisionOff && (1 << other.gameObject.layer & 1 << LayerMask.NameToLayer(Define.PLAYER)) != 0)
        {
            CallAttackEvent();
        }
    }

    private void playerAttack()
    {
        if (!collisionOff)
        {
            CharacterManager.Instance.Player.battle.ChangeHealth(-1, transform.position);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.PLAYER),LayerMask.NameToLayer(Define.ENEMY), true);
            StartCoroutine(collisionDelay());
        }
    }

    public void CallAttackEvent()
    {
        AttackEvent?.Invoke();
    }
    
    private IEnumerator collisionDelay()
    {
        yield return invincible;
        //SetLayerCollisionMatrix(LayerMask.NameToLayer(Define.PLAYER),LayerMask.NameToLayer(Define.ENEMY),true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.PLAYER),LayerMask.NameToLayer(Define.ENEMY), false);
    }
}
