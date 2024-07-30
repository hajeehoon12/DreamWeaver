using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SpinBlade : StateMachineBehaviour
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
        samurai.BackStepAttackEnd();

        if (samurai.isPhase1) samurai.Discrimination();
        else samurai.DoDashAttack();


    }

}