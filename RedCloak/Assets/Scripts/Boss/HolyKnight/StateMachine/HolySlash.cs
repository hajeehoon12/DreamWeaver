using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolySlash : StateMachineBehaviour
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
        AudioManager.instance.PlayHoly("LongBattleCry", 0.1f);
        holy.HolySlashEffects();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Dir = holy.isFlip ? -1f : 1f;
        isWall = Physics2D.Raycast(holy.transform.position + new Vector3(2, 0), Vector2.right * Dir, 2f, groundLayerMask);
        if (holy.slashMove && !isWall)
        {
            holy.transform.position += new Vector3(10 * (Time.deltaTime * Dir * 2f), 0);
        }

    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        holy.HolySlashEnd();
    }

}
