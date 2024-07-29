using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiRun : StateMachineBehaviour
{
    Samurai samurai;
    float Dir;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (samurai == null)
        {
            samurai = animator.GetComponent<Samurai>();
        }
        Dir = samurai.isFlip ? -1 : 1;
        samurai.canFlip = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        samurai.transform.position += new Vector3(Dir * Time.deltaTime * 15, 0, 0);
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        samurai.canFlip = true;
    }

}
