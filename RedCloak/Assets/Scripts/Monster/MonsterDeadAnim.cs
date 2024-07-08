using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeadAnim : StateMachineBehaviour
{
    private Monster _monster;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.enabled = false;
        _monster = animator.GetComponent<Monster>();
        _monster.StartCoroutine(_monster.SpawnLight());
    }
}
