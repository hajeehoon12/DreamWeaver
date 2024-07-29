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
        samurai.canFlip = false;
        samurai.ghostDash.makeGhost = true;
        Dir = samurai.isFlip ? -1f : 1f;

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        samurai.transform.position += new Vector3(Dir * Time.deltaTime * 20, 0, 0);
    }




    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        samurai.canFlip = true;
        samurai.ghostDash.makeGhost = false;
        samurai.DashEnd();
        //samurai.DoNormalAttack();

    }

}
