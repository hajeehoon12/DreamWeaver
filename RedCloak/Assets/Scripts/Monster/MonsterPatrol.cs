using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPatrol : MonoBehaviour, IMonsterBehavior
{
    [SerializeField] private Vector3 wallRayPos;
    [SerializeField] private float wallRayDistance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float invincibleTime;

    [SerializeField] private MonsterState _state;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private MonsterAnimation _animation;

    private Vector3 flipAngle = new Vector3(0, 180, 0);

    private WaitForSeconds invincible;
    
    // Start is called before the first frame update
    void Start()
    {
        _state.AddState(MonsterStateEnum.Patrol);
        invincible = new WaitForSeconds(invincibleTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_state.CheckState(MonsterStateEnum.Idle) && !_state.CheckState(MonsterStateEnum.Chase))
        {
            _state.AddState(MonsterStateEnum.Patrol);
            Patrol();
        }
        else if (_state.CheckState(MonsterStateEnum.Idle) || _state.CheckState(MonsterStateEnum.Chase))
        {
            _state.RemoveState(MonsterStateEnum.Patrol);
            _rigidbody.velocity = Vector2.zero;
        }
        
        _animation.SetRunAnim(_rigidbody.velocity.magnitude != 0);
            
        Debug.DrawRay(transform.position + transform.right, transform.up * -1, Color.red);
        Debug.DrawRay(transform.position + wallRayPos, transform.right * wallRayDistance, Color.red);
    }

    void Patrol()
    {
        Vector2 velocity = transform.right * moveSpeed;
        velocity.y = _rigidbody.velocity.y;
        //_rigidbody.MovePosition(_rigidbody.position + (Vector2)(transform.right * moveSpeed * Time.deltaTime));
        _rigidbody.velocity = velocity;
        
        
        bool CliffRay = Physics2D.Raycast(transform.position + transform.right, transform.up, -2f, 1 << 9);
        bool WallRay = Physics2D.Raycast(transform.position + wallRayPos, transform.right, wallRayDistance, 1 << 9);
        if (!CliffRay || WallRay)
        {
            Debug.Log("Turn Around!");
            Flip();
        }
    }

    private void Flip()
    {
        transform.eulerAngles = transform.eulerAngles.y == 0
            ? transform.eulerAngles + flipAngle
            : transform.eulerAngles - flipAngle;
    }

    
    
    public INode.State StartBehavior()
    {
        throw new System.NotImplementedException();
    }

    public void SetAnimation()
    {
        throw new System.NotImplementedException();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((1 << other.gameObject.layer & 1 << 11) != 0)
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
