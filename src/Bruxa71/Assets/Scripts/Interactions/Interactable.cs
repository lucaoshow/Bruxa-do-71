using UnityEngine;

namespace Root.Interactions
{
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField] private GameObject interactionIcon;
        protected bool stillInteractable = true;
        protected bool interactWhenCollide = false;
        protected InteractionTypes interactionType;
        
        public virtual void Interact() { }

        public virtual void Start()
        {
            this.interactionIcon = Instantiate(this.interactionIcon, this.transform.position + new Vector3(0.1f, 0.1f, 0), Quaternion.identity, this.transform);
            this.interactionIcon.SetActive(false);
        }

        public InteractionTypes GetInteractionType() 
        {
            return this.interactionType;
        }

        public virtual bool CanInteract()
        {
            return this.stillInteractable;
        }

        public bool InteractOnCollision()
        {
            return this.interactWhenCollide;
        }
        
        public void InteractionIconVisible(bool visible)
        {
            if (this.stillInteractable)
            {
                this.interactionIcon.SetActive(visible);
            }
        }
        
        public void DisableInteraction()
        {
            this.InteractionIconVisible(false);
            this.stillInteractable = false;
        }

        public void EnableInteraction()
        {
            this.stillInteractable = true;
        }
    }       
}