using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Detect : Action
{
    public SharedVector2 DetectSize;
    public SharedVector2 DetectBoxPos;
    public SharedTransform Target;

    private TaskStatus current = TaskStatus.Running;

	public override TaskStatus OnUpdate()
    {
        return current;
    }

    public override void OnFixedUpdate()
    {
        RaycastHit2D hit = Physics2D.BoxCast((Vector2)transform.position + DetectBoxPos.Value, DetectSize.Value, 0, Vector2.zero, 0, 1 << LayerMask.NameToLayer(Define.PLAYER));
        if (hit)
        {
            Target.Value = hit.transform;
        }
        else
        {
            Target.Value = null;
        }
        current = hit ? TaskStatus.Success : TaskStatus.Failure;
    }
    
    public override void OnEnd()
    {
        current = TaskStatus.Running;
    }
}