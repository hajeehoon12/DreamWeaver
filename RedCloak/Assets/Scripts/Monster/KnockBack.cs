using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class KnockBack : Action
{
    private float time;
    private Rigidbody2D _rigidbody;
    public SharedFloat KnockbackPower;
    
	public override void OnStart()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(_rigidbody.mass * -KnockbackPower.Value * transform.right, ForceMode2D.Impulse);
        time = 0f;
    }

	public override TaskStatus OnUpdate()
	{
        if (time < 0.2)
        {
            time += Time.deltaTime;
            return TaskStatus.Running;
        }
		return TaskStatus.Success;
	}
}