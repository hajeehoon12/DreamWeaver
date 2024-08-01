using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class Patrol : Action
    {
        private Rigidbody2D _rigidbody;
        private Vector2 velocity;

        public SharedFloat Speed;
        public SharedBool KnockBack;

        private TaskStatus current = TaskStatus.Running;
        
        public override void OnAwake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public override TaskStatus OnUpdate()
        {
            return current;
        }
        
        public override void OnFixedUpdate()
        {
            velocity = Speed.Value * transform.right;
            velocity.y = _rigidbody.velocity.y;
            _rigidbody.velocity = velocity;

            current = TaskStatus.Success;
        }

        public override void OnEnd()
        {
            current = TaskStatus.Running;
        }
    }
}
