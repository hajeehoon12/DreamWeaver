using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class CheckWallCliff : Action
    {
        [SerializeField] private LayerMask wallLayer;
        private TaskStatus current = TaskStatus.Running;
        public SharedVector2 WallRayPos;
        public SharedFloat WallDistance;
        private bool needFlip;
        private bool ground;

        public override TaskStatus OnUpdate()
        {
            return current;
        }

        public override void OnFixedUpdate()
        {
            ground = Physics2D.Raycast(transform.position, transform.up, -0.1f, wallLayer);
            if (!ground)
            {
                current = TaskStatus.Failure;
                return;
            }
            
            bool wall = Physics2D.Raycast((Vector2)transform.position + WallRayPos.Value, transform.right, WallDistance.Value,
                wallLayer);
            bool cliff = Physics2D.Raycast(transform.position + transform.right, transform.up, -0.1f,
                wallLayer);
            
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
