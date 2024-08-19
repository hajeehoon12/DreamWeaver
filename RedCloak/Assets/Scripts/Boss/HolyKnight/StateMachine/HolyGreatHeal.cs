using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyGreatHeal : StateMachineBehaviour
{
    HolyKnight holy;
    float Dir;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (holy == null)
        {
            holy = animator.GetComponent<HolyKnight>();
        }
        holy.canFlip = false;
        holy.isInvincible = true;
        holy.Aura.SetActive(true);

        AudioManager.instance.PlayHoly("HolyHeal", 0.25f);
        holy.GreatHealStart();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        holy.GreatHealEnd();
    }

}
