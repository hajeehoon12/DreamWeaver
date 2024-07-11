using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class CheckWallCliff : Action
    {
        private TaskStatus current = TaskStatus.Running;
        public SharedVector2 WallRayPos;
        public SharedFloat WallDistance;
        private bool needFlip;

        public override TaskStatus OnUpdate()
        {
            return current;
        }

        public override void OnFixedUpdate()
        {
            bool ground = Physics2D.Raycast(transform.position, transform.up, -1, 1<<LayerMask.NameToLayer(Define.FLOOR));
            if (!ground)
            {
                current = TaskStatus.Failure;
                return;
            }
            
            bool wall = Physics2D.Raycast((Vector2)transform.position + WallRayPos.Value, transform.right, WallDistance.Value,
                1 << LayerMask.NameToLayer(Define.FLOOR));
            bool cliff = Physics2D.Raycast(transform.position + transform.right, transform.up, -3f,
                1 << LayerMask.NameToLayer(Define.FLOOR));
            
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
