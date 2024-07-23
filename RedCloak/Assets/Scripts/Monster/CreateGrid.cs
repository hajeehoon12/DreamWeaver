using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CreateGrid : Action
{
    public SharedVector2 GridSize;
    public SharedVector2 BottomLeft;
    public SharedVector2 UpperRight;
    
	public override void OnStart()
    {
        BottomLeft.Value = new Vector2(transform.position.x - GridSize.Value.x / 2, transform.position.y - GridSize.Value.y / 2);
        UpperRight.Value = new Vector2(transform.position.x + GridSize.Value.x / 2,
            transform.position.y + GridSize.Value.y / 2);
    }

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}
}