using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class checkAttack : Action
{
    [SerializeField] private MonsterController _controller;

	public override TaskStatus OnUpdate()
    {
        return _controller.OnAttack ? TaskStatus.Running : TaskStatus.Success;
    }
}