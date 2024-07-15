using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private WaitForSeconds invincible;
    public IMobAttack MobAttack;

    private bool collisionOff = false;
    public bool OnAttack { get; set; } = false;

    private void Start()
    {
        invincible = new WaitForSeconds(CharacterManager.Instance.Player.battle.healthChangeDelay);
    }

    public void Attack()
    {
        if (MobAttack != null)
        {
            if (MobAttack.PerformAttack() && !collisionOff)
            {
                collisionOff = true;
                //SetLayerCollisionMatrix(LayerMask.NameToLayer(Define.PLAYER),LayerMask.NameToLayer(Define.ENEMY),false);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.PLAYER),LayerMask.NameToLayer(Define.ENEMY), true);
                StartCoroutine(collisionDelay());
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!collisionOff && (1 << other.gameObject.layer & 1 << LayerMask.NameToLayer(Define.PLAYER)) != 0)
        {
            collisionOff = true;
            other.gameObject.GetComponent<PlayerBattle>().ChangeHealth(-1, transform.position);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.PLAYER),LayerMask.NameToLayer(Define.ENEMY), true);
            //SetLayerCollisionMatrix(LayerMask.NameToLayer(Define.PLAYER),LayerMask.NameToLayer(Define.ENEMY),false);
            StartCoroutine(collisionDelay());
        }
    }
    
    private void SetLayerCollisionMatrix(int a, int b, bool enable)
    {
        int aMask = Physics2D.GetLayerCollisionMask(a);

        aMask = enable ? aMask | (1 << b) : aMask & ~(1 << b);
        
        Physics2D.SetLayerCollisionMask(a,aMask);
    }
    
    private IEnumerator collisionDelay()
    {
        yield return invincible;
        //SetLayerCollisionMatrix(LayerMask.NameToLayer(Define.PLAYER),LayerMask.NameToLayer(Define.ENEMY),true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.PLAYER),LayerMask.NameToLayer(Define.ENEMY), false);
        collisionOff = false;
    }
}
