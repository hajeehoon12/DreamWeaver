using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

public class HolyBackDash : StateMachineBehaviour
{
    HolyKnight holy;
    //float Dir;
    //bool isWall;
    //public LayerMask groundLayerMask;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (holy == null)
        {
            holy = animator.GetComponent<HolyKnight>();
        }

        if (!holy.TempStage)
        {
            holy.EndBackDash();
        }
        else
        {
            holy.DisAppear();
        }
        AudioManager.instance.PlayHoly("BackDash", 0.1f);
        //holy.canFlip = false;
        //AudioManager.instance.PlayHoly("BattleCry1", 0.1f, 0.9f);
    }

 



    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        holy.HeavyAttackDown();
    }

}