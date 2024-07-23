using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckDistance : Action
{
    public SharedTransform player;
    public SharedBool Meele;
    public SharedFloat MeeleRange;
    
	public override TaskStatus OnUpdate()
    {
        Meele.Value = (player.Value.position - transform.position).magnitude <=
                      MeeleRange.Value;
		return TaskStatus.Success;
	}
}