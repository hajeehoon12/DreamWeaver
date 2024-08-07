using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackEnd : StateMachineBehaviour
{
    private MonsterController _controller;
    private bool init = false;


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!init)
        {
            _controller = animator.GetComponent<MonsterController>();
            init = true;
        }

        _controller.OnAttack = false;
    }
}
