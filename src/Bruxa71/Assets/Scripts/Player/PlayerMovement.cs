using UnityEngine;

namespace Root.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigidBody;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float maxSecondsToStop;
        [SerializeField] private float secondsToTurn;

        private float secondsToStop;
        private float timeStopping;
        private float lastX;
        private bool stopped = true;

        private void Start()
        {
            this.timeStopping = this.secondsToStop;   
        }

        public void Stop()
        {
            this.Deaccelerate(this.secondsToStop);
            if (this.stopped)
            {
                // play idle animation
            }
        }

        public void Move(Vector2 moveDir)
        {
            if (!this.stopped && this.lastX != moveDir.x)
            {
                this.Turn();
            }
            else
            {
                this.rigidBody.velocity = moveDir * this.moveSpeed;

                this.lastX = moveDir.x;
                this.timeStopping = 0;
                this.stopped = false;
            }

            this.secondsToStop = Mathf.Clamp(this.secondsToStop + Time.deltaTime, 0, this.maxSecondsToStop);
        }

        private void Turn()
        {
            this.Deaccelerate(this.secondsToTurn);

        }

        private void Deaccelerate(float maxSeconds)
        {
            if (this.timeStopping <= maxSeconds)
            {
                this.rigidBody.velocity = new Vector2(Mathf.Lerp(this.moveSpeed * this.lastX, 0, this.timeStopping / this.secondsToStop), 0);
                this.timeStopping += Time.deltaTime;
                // play sliding animation
            }
            else
            {
                this.rigidBody.velocity = Vector2.zero;
                this.stopped = true;
                this.secondsToStop = 0;
            }
        }
    }
}
