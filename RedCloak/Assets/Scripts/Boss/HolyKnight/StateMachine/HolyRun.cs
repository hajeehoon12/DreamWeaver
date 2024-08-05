using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class HolyRun : StateMachineBehaviour
{
    HolyKnight holy;
    float Dir;
    bool isWall;
    public LayerMask groundLayerMask;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (holy == null)
        {
            holy = animator.GetComponent<HolyKnight>();
        }
        holy.canFlip = false;
        //holy.ghostDash.makeGhost = true;
        Dir = holy.isFlip ? -1f : 1f;
        //AudioManager.instance.PlayHoly("BattleCry1", 0.1f, 0.9f);

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        isWall = Physics2D.Raycast(holy.transform.position + new Vector3(2,0), Vector2.right * Dir, 4f, groundLayerMask);
        if (!isWall)
        {
            holy.transform.position += new Vector3(Dir * Time.deltaTime * 12, 0, 0);
        }
        else
        {
            holy.RunEnd();
            //Debug.Log("Meet wall");
        }

    }




    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        holy.canFlip = true;
        //holy.ghostDash.makeGhost = false;
        holy.RunEnd();


    }

}