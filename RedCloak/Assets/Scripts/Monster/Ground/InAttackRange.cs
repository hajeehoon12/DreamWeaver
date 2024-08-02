using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class InAttackRange : Action
{
    private TaskStatus current;

    public SharedVector2 AttackRayPos;
    public SharedFloat AttackDistance;
    
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
        if (Physics2D.Raycast((Vector2)transform.position + AttackRayPos.Value, transform.right, AttackDistance.Value,
                1 << LayerMask.NameToLayer(Define.PLAYER)))
        {
            current = TaskStatus.Success;
        }
        else
        {
            current = TaskStatus.Failure;
        }
    }
}