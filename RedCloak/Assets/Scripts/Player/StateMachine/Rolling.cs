using AnimationImporter.PyxelEdit;
using DG.Tweening;
using UnityEngine;

public class Rolling : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController controller = animator.GetComponent<PlayerController>();
        float dir = controller.GetComponent<SpriteRenderer>().flipX ? -1f : 1f;
        controller.transform.DOMoveX(controller.transform.position.x + controller.maxSpeed * dir * 0.5f, 0.5f);
    }
}
