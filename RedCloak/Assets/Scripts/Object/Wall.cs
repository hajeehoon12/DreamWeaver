using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    PlayerController controller;
    private static readonly int isWallClimbing = Animator.StringToHash("IsWallClimbing");
    public float playerSlopeSpeed = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out controller))
        {
            controller.slopeSpeed = playerSlopeSpeed;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log("Get Enter");
        if (collision.gameObject.TryGetComponent<PlayerController>(out controller))
        {
            //Debug.Log("Getcontroller");
            if (controller.animator.GetBool(isWallClimbing))
            {
                //Debug.Log("WallClimbDown");
                controller.rigid.velocity = new Vector2(controller.rigid.velocity.x, -playerSlopeSpeed);
                //collision.gameObject.GetComponent<CapsuleCollider2D>().sharedMaterial.friction = 1f;
                //Debug.Log(controller.rigid.velocity);
            }

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out controller))
        {
            controller.slopeSpeed = controller.originSlopeSpeed;
        }

    }
}
