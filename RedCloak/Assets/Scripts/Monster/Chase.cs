using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Chase : Action
{
    private Rigidbody2D _rigidbody;

    public SharedTransform Target;
    
	public override void OnStart()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}
}