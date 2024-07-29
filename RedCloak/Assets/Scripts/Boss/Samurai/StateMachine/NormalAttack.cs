using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class NormalAttack : StateMachineBehaviour
{
    Samurai samurai;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (samurai == null)
        {
            samurai = animator.GetComponent<Samurai>();
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        samurai.NormalAttackEnd();
        samurai.Discrimination();
    }

}
