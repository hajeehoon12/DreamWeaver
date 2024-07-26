using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiDash : StateMachineBehaviour
{
    Samurai samurai;
    float Dir;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (samurai == null)
        {
            samurai = animator.GetComponent<Samurai>();
        }

    }




    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        samurai.canFlip = false;
        samurai.ghostDash.makeGhost = false;
        samurai.DashAttackEnd();

    }

}
