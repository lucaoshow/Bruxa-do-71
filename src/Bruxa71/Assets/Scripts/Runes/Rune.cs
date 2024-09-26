using Root.Player;
using UnityEditor;
using UnityEngine;

namespace Root.Runes
{
    [ExecuteInEditMode]
    public abstract class Rune : MonoBehaviour
    {
        [SerializeField] private float cooldown;
        [SerializeField] private RuneEffect runeEffect;

        private bool inCooldown = false;
        private float timeSinceLastUse = 0f;

        private void Awake()
        {
            if (!EditorApplication.isPlaying) 
            {
                this.SetupInspectorComponents();
            }
        }

        private void Update() 
        {
            if (this.inCooldown)
            {
                if (this.timeSinceLastUse >= this.cooldown) 
                {
                    this.timeSinceLastUse = 0f - Time.deltaTime;
                    this.inCooldown = false;
                }

                this.timeSinceLastUse += Time.deltaTime;
            }
        }

        public RuneEffect GetRuneEffect() 
        {
            return this.runeEffect;
        }

        public virtual void Activate(PlayerRunesManager player, Vector2 playerLookingDir)
        {
            if (!this.inCooldown)
            {
                this.runeEffect.Apply(player, playerLookingDir);
            }
        }

        private void SetupInspectorComponents()
        {
            if (!this.gameObject.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                this.gameObject.AddComponent<SpriteRenderer>();
            }
            
            if (!this.gameObject.TryGetComponent(out Animator animator))
            {
                this.gameObject.AddComponent<Animator>();
            }

            Debug.Log("Added components for UI sprite and animation.");
        }

    }
}
