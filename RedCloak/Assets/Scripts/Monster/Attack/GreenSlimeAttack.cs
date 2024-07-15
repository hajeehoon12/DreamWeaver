using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSlimeAttack : MonoBehaviour, IMobAttack
{
    [SerializeField] private Animator _animator;
    [SerializeField] private MonsterController _controller;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private bool isElite;

    private PlayerController _playerController;
    private SpriteRenderer playerDirection;
    private Animator playerMoving;

    private bool onGround = true;

    private void OnEnable()
    {
        _controller.MobAttack = this;
        playerTransform = CharacterManager.Instance.Player.transform;
    }

    private void Start()
    {
        _playerController = CharacterManager.Instance.Player.GetComponent<PlayerController>();
        playerDirection = CharacterManager.Instance.Player.GetComponent<SpriteRenderer>();
        playerMoving = CharacterManager.Instance.Player.GetComponent<Animator>();
    }

    public bool PerformAttack()
    {
        AudioManager.instance.PlaySFX("SlimeAttack", 0.2f);
        float predict;
        
        if (playerMoving.GetBool("IsRunning") && isElite)
        {
            predict = playerDirection.flipX ? -1 : 1;
            predict *= _playerController.maxSpeed * Time.fixedDeltaTime * 30;
        }
        else
        {
            predict = 0;
        }
        
        float x = (playerTransform.position.x - transform.position.x + predict) / 2;
        
        Vector2 centerPos = new Vector2(x, 5);
        JumpForce(centerPos);
        return false;
    }

    private void JumpForce(Vector2 maxHeightPos)
    {
        float v_y = Mathf.Sqrt(2 * _rigidbody.gravityScale * -Physics2D.gravity.y * maxHeightPos.y);
        // 포물선 운동 법칙 적용
        float v_x = maxHeightPos.x * v_y / (2 * maxHeightPos.y);

        Vector2 force = _rigidbody.mass * (new Vector2(v_x, v_y) - _rigidbody.velocity);
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Define.FLOOR) && !onGround)
        {
            _animator.SetTrigger("Fall");
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Define.FLOOR) && Physics2D.Raycast(transform.position, transform.up, -0.1f, 1<<LayerMask.NameToLayer(Define.FLOOR)))
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
