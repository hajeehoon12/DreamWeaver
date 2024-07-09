using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttack : MonoBehaviour, IMobAttack
{
    [SerializeField] private MonsterController _controller;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform playerTransform;

    private bool onGround;

    private void Start()
    {
        _controller.MobAttack = this;
        playerTransform = CharacterManager.Instance.Player.transform;
    }

    public bool PerformAttack()
    {
        float x = (playerTransform.position.x - transform.position.x) / 2;
        Vector2 centerPos = new Vector2(x, 5);
        if (onGround)
            JumpForce(centerPos);
        return true;
    }

    private void JumpForce(Vector2 maxHeightPos)
    {
        float v_y = Mathf.Sqrt(2 * _rigidbody.gravityScale * -Physics2D.gravity.y * maxHeightPos.y);
        // 포물선 운동 법칙 적용
        float v_x = maxHeightPos.x * v_y / (2 * maxHeightPos.y);

        Vector2 force = _rigidbody.mass * (new Vector2(v_x, v_y) - _rigidbody.velocity);
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Define.FLOOR))
        {
            onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Define.FLOOR))
        {
            onGround = false;
        }
    }
}
