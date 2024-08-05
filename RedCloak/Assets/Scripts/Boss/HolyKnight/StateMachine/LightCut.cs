using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCut : StateMachineBehaviour
{
    HolyKnight holy;
    //float Dir;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (holy == null)
        {
            holy = animator.GetComponent<HolyKnight>();
        }
        AudioManager.instance.PlayHoly("LightCut", 0.1f);
        

    }




    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        holy.Discrimination();
    }

}
