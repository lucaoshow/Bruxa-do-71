using UnityEngine;

namespace Root.Interactions.Dialogues
{
    public class DialogueSource : Interactable
    {
        [SerializeField] private TextAsset inkJson;
        [SerializeField] private bool openDialogueOnCollision = false;
        private bool firstInteraction;

        public override void Start()
        {
            base.Start();
            this.interactWhenCollide = this.openDialogueOnCollision;
            this.firstInteraction = true;
        }

        public override void Interact()
        {
            if (this.firstInteraction)
            {
                DialogueManager.Instance.StartDialogue(this.inkJson);
                this.firstInteraction = false;
            }
            else if (this.CanInteract())
            {
                DialogueManager.Instance.ContinueDialogue();
                this.stillInteractable = !DialogueManager.Instance.EndedDialogue();
            }
        }
    }
}