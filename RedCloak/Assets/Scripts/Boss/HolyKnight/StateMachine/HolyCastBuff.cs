using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyCastBuff : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioManager.instance.PlayHoly("CastBuff", 0.05f);

    }

}