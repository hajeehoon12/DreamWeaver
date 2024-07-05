using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class CheckWallCliff : Action
    {
        private TaskStatus current = TaskStatus.Running;
        public SharedVector2 WallRayPos;
        private bool needFlip;

        public override TaskStatus OnUpdate()
        {
            return current;
        }

        public override void OnFixedUpdate()
        {
            bool wall = Physics2D.Raycast((Vector2)transform.position + WallRayPos.Value, transform.right, 3f,
                1 << LayerMask.NameToLayer("Floor"));
            bool cliff = Physics2D.Raycast(transform.position + transform.right, transform.up, -3f,
                1 << LayerMask.NameToLayer("Floor"));
            
            if (wall || !cliff)
            {
                needFlip = true;
            }
            else
            {
                needFlip = false;
            }

            current = needFlip ? TaskStatus.Success : TaskStatus.Failure;
        }
        
        public override void OnEnd()
        {
            current = TaskStatus.Running;
        }
    }
}
