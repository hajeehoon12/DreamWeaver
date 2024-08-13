using DG.Tweening;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Rolling : StateMachineBehaviour
{
    private static readonly int isRolling = Animator.StringToHash("IsRolling");

    PlayerController controller;
    float dir = 1f;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<PlayerController>();
        dir = controller.GetComponent<SpriteRenderer>().flipX ? -1f : 1f;
        //controller.transform.DOMoveX(controller.transform.position.x + controller.maxSpeed * dir * 0.5f, 0.5f);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (controller.Rolling)
        {
            controller.transform.position += new Vector3(dir, 0) * 1.2f * controller.maxSpeed * Time.deltaTime;
        }
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        controller.Rolling = false;
        animator.SetBool(isRolling, false);
    }

}
