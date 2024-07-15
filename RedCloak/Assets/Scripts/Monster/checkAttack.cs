using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class checkAttack : Action
{
    public SharedBool Attacking;

	public override TaskStatus OnUpdate()
    {
        return Attacking.Value ? TaskStatus.Running : TaskStatus.Success;
    }
}