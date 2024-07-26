using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiDashAttack : StateMachineBehaviour
{
    Samurai samurai;
    //float Dir;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (samurai == null)
        {
            samurai = animator.GetComponent<Samurai>();
        }

    }




    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        samurai.canFlip = true;
        samurai.ghostDash.makeGhost = false;
        samurai.DashAttackEnd();

    }

}
