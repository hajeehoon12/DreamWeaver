using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEditor.Rendering;

public class FlyPatrol : Action
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private LayerMask wallLayer;
    public SharedVector2 destination;
    public SharedVector2 RayPosition;
    public SharedFloat MoveSpeed;
    public SharedFloat RayDistance;
    public SharedBool NeedRandom;
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
        Vector2 direction = (destination.Value - (Vector2)transform.position).normalized;
        if (Physics2D.CircleCast((Vector2)transform.position + RayPosition.Value, RayDistance.Value, direction, MoveSpeed.Value * Time.fixedDeltaTime,
                wallLayer))
        {
            NeedRandom.Value = true;
            current = TaskStatus.Success;
            return;
        }
            

        _rigidbody.position = Vector2.MoveTowards(_rigidbody.position, destination.Value, MoveSpeed.Value * Time.fixedDeltaTime);
        NeedRandom.Value = _rigidbody.position == destination.Value ? true : false;
        
        current = TaskStatus.Success;
    }
}