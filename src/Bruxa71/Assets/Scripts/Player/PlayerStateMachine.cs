using UnityEngine;

namespace Root.Player
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerRunesManager runesManager;

        private Vector2 moveDirection;
        private enum PlayerStates
        {
            Idle,
            Walking
        }
        private PlayerStates state;

        private void Update()
        {
            this.ChangeStateBasedOnInput();

            switch (this.state) {
                case PlayerStates.Idle:
                    this.playerMovement.Stop();
                    break;

                case PlayerStates.Walking:
                    this.playerMovement.Move(this.moveDirection);
                    break;
            }
        }

        private void ChangeStateBasedOnInput()
        {
            float movement = Input.GetAxisRaw("Horizontal");

            if (movement != 0)
            {
                this.state = PlayerStates.Walking;
                this.moveDirection = new Vector2(movement, 0);
            }
            else
            {
                this.state = PlayerStates.Idle;
            }
        }
        
    }
}
