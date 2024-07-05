using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private float invincibleTime;
    
    private WaitForSeconds invincible;
    public IMobAttack MobAttack;

    private void Start()
    {
        invincible = new WaitForSeconds(invincibleTime);
    }

    public void Attack()
    {
        if (MobAttack != null)
        {
            if (MobAttack.Attack())
            {
                SetLayerCollisionMatrix(gameObject.layer, LayerMask.NameToLayer("Player"), false);
                StartCoroutine(collisionDelay());
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((1 << other.gameObject.layer & 1 << LayerMask.NameToLayer("Player")) != 0)
        {
            other.gameObject.GetComponent<PlayerController>().OnGetAttacked();
            SetLayerCollisionMatrix(gameObject.layer, other.gameObject.layer, false);
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
        SetLayerCollisionMatrix(7,11,true);
    }
}
