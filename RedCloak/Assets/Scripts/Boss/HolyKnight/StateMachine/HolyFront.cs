using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyFront : StateMachineBehaviour
{
    HolyKnight holy;
    //float Dir;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (holy == null)
        {
            holy = animator.GetComponent<HolyKnight>();
        }
        holy.canFlip = false;
        AudioManager.instance.PlayHoly("BattleCry1", 0.1f, 0.9f);

    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        holy.FrontEnd();
    }

}