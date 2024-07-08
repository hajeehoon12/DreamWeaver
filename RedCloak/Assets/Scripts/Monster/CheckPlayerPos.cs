using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckPlayerPos : Action
{
    private bool needFlip;

    public SharedTransform Target;

	public override TaskStatus OnUpdate()
    {
        Vector2 directionToPlayer = (Target.Value.position - transform.position);
        Vector2 forward = transform.right;

        if (Vector2.Dot(forward, directionToPlayer) > 0)
            needFlip = false;
        else
            needFlip = true;
        
        return needFlip ? TaskStatus.Success : TaskStatus.Failure;
    }
}