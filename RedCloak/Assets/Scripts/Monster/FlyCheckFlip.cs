using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class FlyCheckFlip : Action
{
    public SharedVector2 destination;
    private bool needFlip;

    public override TaskStatus OnUpdate()
    {
        needFlip = !(Vector2.Dot(transform.right, destination.Value - (Vector2)transform.position) > 0);
        return needFlip ? TaskStatus.Success : TaskStatus.Failure;
	}
}