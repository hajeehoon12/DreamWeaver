using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckDistance : Action
{
    public SharedBool Meele;
    public SharedFloat MeeleRange;

	public override TaskStatus OnUpdate()
    {
        Meele.Value = (CharacterManager.Instance.Player.transform.position - transform.position).magnitude <=
                      MeeleRange.Value;
		return TaskStatus.Success;
	}
}