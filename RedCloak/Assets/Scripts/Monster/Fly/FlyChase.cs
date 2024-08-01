using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;

public class FlyChase : Action
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SharedTransform player;
    [SerializeField] private SharedFloat moveSpeed;
    [SerializeField] private SharedBool chase;
    [SerializeField] private LayerMask wallLayer;
    private TaskStatus current;
    
	public override void OnStart()
    {
        current = TaskStatus.Running;
    }

	public override TaskStatus OnUpdate()
    {
        
        return current;
    }

    public override void OnFixedUpdate()
    {
        if (Physics2D.Raycast(transform.position + new Vector3(0, 0.5f), (player.Value.position - transform.position).normalized, 1.5f, wallLayer))
        {
            chase.Value = false;
            current = TaskStatus.Failure;
            return;
        }
            
        chase.Value = true;
        _rigidbody.position = Vector2.MoveTowards(_rigidbody.position, player.Value.position, (moveSpeed.Value+3) * Time.fixedDeltaTime);
        //_rigidbody.position = Vector2.Lerp(_rigidbody.position, player.Value.position, moveSpeed.Value * Time.fixedDeltaTime);
        current = TaskStatus.Success;
    }
}