using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Jumping : StateMachineBehaviour
{
    PlayerController controller;
    

    private static readonly int isFalling = Animator.StringToHash("IsFalling");
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<PlayerController>();
        animator.SetBool(isFalling, false);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (controller.canDoubleJump)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //controller.GetComponent<Rigidbody2D>().gravityScale = 2f;
                controller.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                controller.isGrounded = true;
                controller.OnJump();
                controller.canDoubleJump = false;
                controller.DoubleJump();
            }
            //else
            //{
            //    controller.GetComponent<Rigidbody2D>().gravityScale = 3f;
            //}
        }
       
        controller.rigid.gravityScale = controller.isLongJump ? 2.0f : 4.0f;
        

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!controller.ghostDash.makeGhost)
        controller.GetComponent<Rigidbody2D>().gravityScale = 2f;    
    }

}
