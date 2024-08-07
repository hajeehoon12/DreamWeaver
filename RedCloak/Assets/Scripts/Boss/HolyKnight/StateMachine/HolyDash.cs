using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyDash : StateMachineBehaviour
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
        holy.ghostDash.makeGhost = true;
        Dir = holy.isFlip ? -1f : 1f;
        AudioManager.instance.PlayHoly("JumpDash", 0.2f);
        //AudioManager.instance.PlayHoly("BattleCry1", 0.1f, 0.9f);

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        holy.transform.position += new Vector3(Dir * Time.deltaTime * 20, 0, 0);
    }




    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        holy.canFlip = true;
        holy.ghostDash.makeGhost = false;
        holy.DashEnd();
        

    }

}
