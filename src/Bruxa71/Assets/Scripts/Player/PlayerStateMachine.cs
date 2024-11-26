using Root.Interactions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Root.Player
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerInteractionHandler interactionHandler;
        [SerializeField] private PlayerRunesManager runesManager;
        [SerializeField] private InputActionAsset inputActions;
        private InputAction moveAction;
        private Vector2 moveDirection;
        private bool blockInput = false;
        private enum PlayerStates
        {
            Idle,
            Walking,
            Dialoguing
        }
        private PlayerStates state;

        private void Awake() 
        {
            this.moveAction = this.inputActions.FindActionMap("Movement").FindAction("move");
            this.moveAction.Enable();
        }

        private void Update()
        {
            if (!this.blockInput)
            {
                this.ChangeStateBasedOnInput();
            }

            switch (this.state) {
                case PlayerStates.Idle:
                    this.playerMovement.Stop();
                    break;

                case PlayerStates.Walking:
                    this.playerMovement.Move(this.moveDirection);
                    break;
                
                case PlayerStates.Dialoguing:
                    this.playerMovement.Stop();
                    if (!this.interactionHandler.CanInteract())
                    {
                        this.blockInput = false;
                        return;
                    }
                    
                    if (this.InteractionKeyPressed())
                    {
                        this.interactionHandler.Interact();
                    }
                    
                    break;
            }
        }

        private bool InteractionKeyPressed()
        {
            return Input.GetKeyDown(KeyCode.F);
        }

        private void ChangeStateBasedOnInput()
        {
            this.moveDirection = this.moveAction.ReadValue<Vector2>().normalized;

            bool tryingToInteract = this.InteractionKeyPressed();

            if (tryingToInteract && this.interactionHandler.CanInteract())
            {
                this.blockInput = true;
                this.state = this.InteractionTypeToPlayerState(this.interactionHandler.GetInteractionType());
                return;
            }

            if (this.moveDirection != Vector2.zero)
            {
                this.state = PlayerStates.Walking;
            }
            else
            {
                this.state = PlayerStates.Idle;
            }

        }

        public void OnForcedInteraction(InteractionTypes interactionType)
        {
            this.blockInput = true;
            this.state = this.InteractionTypeToPlayerState(interactionType);
        }
        
        private PlayerStates InteractionTypeToPlayerState(InteractionTypes interactionType)
        {
            switch (interactionType)
            {
                case InteractionTypes.Dialogue:
                    return PlayerStates.Dialoguing;
                
                default:
                    this.blockInput = false;
                    return PlayerStates.Idle;
            }
        }
    }
}
