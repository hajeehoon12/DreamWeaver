using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class InAttackBound : Action
{
    private TaskStatus current;
    [SerializeField] private float radius;
    
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
        current = Physics2D.CircleCast(transform.position + transform.right + transform.up, radius, Vector2.zero, 0,
            1 << LayerMask.NameToLayer(Define.PLAYER))
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}