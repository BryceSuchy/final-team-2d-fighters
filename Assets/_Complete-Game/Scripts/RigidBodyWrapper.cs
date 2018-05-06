using UnityEngine;

namespace Completed
{
    public class RigidBodyWrapper
    {
        private Rigidbody2D rigidBody;
        private MonoBehaviour parent;
        private bool spying;
        public bool movePositionCalled;
        
        public RigidBodyWrapper(MonoBehaviour parent)
        {
            this.parent = parent;
            rigidBody = parent.GetComponent<Rigidbody2D>();
            spying = false;
            movePositionCalled = false;
        }

        public void startSpying()
        {
            spying = true;
        }

        public void MovePosition(Vector2 destination)
        {
            if (spying)
            {
                movePositionCalled = true;
            } else
            {
                rigidBody.MovePosition(destination);
            }
        }
    }
}