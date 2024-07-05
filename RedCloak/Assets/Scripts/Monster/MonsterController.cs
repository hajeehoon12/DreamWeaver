using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private float invincibleTime;
    [SerializeField] private Vector2 attackRayPos;
    [SerializeField] private float attackRange;
    private WaitForSeconds invincible;

    private void Start()
    {
        invincible = new WaitForSeconds(invincibleTime);
    }

    public void Attack()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + attackRayPos, transform.right, attackRange,
            1 << LayerMask.NameToLayer("Player"));
        if (hit)
        {
            hit.collider.GetComponent<PlayerController>().OnGetAttacked();
            SetLayerCollisionMatrix(gameObject.layer, hit.collider.gameObject.layer, false);
            StartCoroutine(collisionDelay());
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
