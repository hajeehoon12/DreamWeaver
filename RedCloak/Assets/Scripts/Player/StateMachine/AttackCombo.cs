using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCombo : StateMachineBehaviour
{
    private static readonly int nextCombo = Animator.StringToHash("NextCombo");
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(nextCombo); // Attack Combo off
        //animator.SetTrigger(nextCombo);
    }
}
