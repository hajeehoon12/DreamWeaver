using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : StateMachineBehaviour
{
    private bool init = false;
    private Collider2D _collider2D;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!init)
        {
            init = true;
            _collider2D = animator.GetComponent<Collider2D>();
        }

        _collider2D.enabled = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _collider2D.enabled = true;
        animator.transform.position = CharacterManager.Instance.Player.transform.position + new Vector3(0, 1.2f);
    }
}
