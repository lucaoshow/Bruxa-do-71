using UnityEngine;
using UnityEngine.InputSystem;

namespace Root.Player
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerRunesManager runesManager;
        [SerializeField] private InputActionAsset inputActions;
        private InputAction moveAction;
        private Vector2 moveDirection;
        private enum PlayerStates
        {
            Idle,
            Walking
        }
        private PlayerStates state;

        private void Awake() 
        {
            this.moveAction = this.inputActions.FindActionMap("Movement").FindAction("move");
            this.moveAction.Enable();
        }

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
            this.moveDirection = this.moveAction.ReadValue<Vector2>().normalized;

            if (this.moveDirection != Vector2.zero)
            {
                this.state = PlayerStates.Walking;
            }
            else
            {
                this.state = PlayerStates.Idle;
            }

        }
        
    }
}
