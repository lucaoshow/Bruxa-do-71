using UnityEngine;

namespace Root.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float maxSecondsToStop;
        private Rigidbody2D rigidBody;
        private Vector2 lastDir;
        private float secondsToStop;
        private float timeStopping;

        private void Start()
        {
            this.rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
            this.timeStopping = this.maxSecondsToStop;   
        }

        public void Stop()
        {
            if (this.timeStopping <= this.secondsToStop)
            {
                this.rigidBody.velocity = Vector2.Lerp(this.moveSpeed * this.lastDir, Vector2.zero, this.timeStopping / this.secondsToStop);
                this.timeStopping += Time.deltaTime;
                // play sliding animation
            }
            else
            {
                this.rigidBody.velocity = Vector2.zero;
                this.secondsToStop = 0;
                // play idle animation
            }
        }

        public void Move(Vector2 moveDir)
        {
            this.rigidBody.velocity = moveDir * this.moveSpeed;

            this.timeStopping = 0;

            this.secondsToStop = Mathf.Clamp(this.secondsToStop + Time.deltaTime, 0, this.maxSecondsToStop);
        }

    }
}
