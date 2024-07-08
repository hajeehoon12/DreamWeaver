using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Flip : Action
{
    private Vector3 flipAngle = new Vector3(0, 180, 0);

	public override TaskStatus OnUpdate()
	{
        transform.eulerAngles = transform.eulerAngles.y == 0
            ? transform.eulerAngles + flipAngle
            : transform.eulerAngles - flipAngle;
		return TaskStatus.Success;
	}
}