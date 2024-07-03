using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Falling : StateMachineBehaviour
{
    PlayerController controller;
    

    //bool canDoubleJump = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<PlayerController>();
        
        
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (controller.canDoubleJump)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                controller.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                controller.isGrounded = true;
                controller.OnJump();
                controller.canDoubleJump = false;
                controller.DoubleJump();

            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

}
