using UnityEngine;

namespace Root.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private float maxSecondsToStop;
        [SerializeField] private Rigidbody2D rigidBody;
        private Vector2 lastDir;
        private float secondsToStop;
        private float timeStopping;

        private void Start()
        {
            this.timeStopping = this.maxSecondsToStop;   
        }

        public void Stop()
        {
            if (this.timeStopping <= this.secondsToStop)
            {
                this.rigidBody.velocity = Vector2.Lerp(this.playerData.moveSpeed * this.lastDir, Vector2.zero, this.timeStopping / this.secondsToStop);
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
            this.rigidBody.velocity = moveDir * this.playerData.moveSpeed;

            this.timeStopping = 0;

            this.secondsToStop = Mathf.Clamp(this.secondsToStop + Time.deltaTime, 0, this.maxSecondsToStop);
        }

    }
}
