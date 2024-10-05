using Root.Player;
using Root.EditorExtensions.PropertyDrawers;
using Root.Projectiles;
using UnityEngine;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace Root.Runes
{
    public abstract class Rune : MonoBehaviour
    {
        [SerializeField] private float cooldown;
        [SerializeField] private RuneEffectTypes effectType;

        [DrawIf("effectType", RuneEffectTypes.EffectOnPlayer)]
        [SerializeField] private EffectOnPlayer effectOnPlayer;

        [DrawIf("effectType", RuneEffectTypes.EffectOnEnemy)]
        [SerializeField] private EffectOnEnemy effectOnEnemy;

        [DrawIf("effectType", RuneEffectTypes.ThrowProjectile)]
        [SerializeField] private Projectile projectile;

        private Animator animator;

        private bool inCooldown = false;
        private float timeSinceLastUse = 0f;
        private int effectApplied = 0;
        private float effectDuration;
        private float effectDurationCount = 0f;
        private SpriteRenderer spriteRenderer;
        private UnityEvent<EffectOnPlayer> applyOnPlayer = new UnityEvent<EffectOnPlayer>();

        private void Start()
        {
            this.effectDuration = this.effectType == RuneEffectTypes.EffectOnPlayer ? this.effectOnPlayer.duration : this.effectOnEnemy.duration;
            this.spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
            this.spriteRenderer.enabled = false;
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

            if (this.effectApplied > 0 ) 
            {
                if (this.effectDurationCount >= this.effectDuration) 
                {
                    this.effectDurationCount = 0f - Time.deltaTime;
                    this.effectApplied--;
                    this.applyOnPlayer.Invoke(-this.effectOnPlayer);
                }

                this.effectDurationCount += Time.deltaTime;
            }
        }

        public void Activate(PlayerRunesManager player, Vector3 playerLookingDir)
        {
            if (this.inCooldown)
            {
                return;
            }

            this.spriteRenderer.enabled = true;
            this.applyOnPlayer.AddListener(player.AddStatus);

            switch (this.effectType)
            {
                case RuneEffectTypes.EffectOnPlayer:
                case RuneEffectTypes.EffectOnEnemy:
                    AnimationClip anim = this.animator.runtimeAnimatorController.animationClips[0];
                    AnimationEvent animEvent = new AnimationEvent();
                    animEvent.functionName = "OnEffectAnimationEnd";
                    animEvent.time = anim.length;
                    anim.AddEvent(animEvent);
                    break;
                
                case RuneEffectTypes.ThrowProjectile:
                    Projectile proj = Instantiate(this.projectile, this.transform.position, Quaternion.identity);
                    proj.SetDirection(playerLookingDir);
                    break;
            }
            
            this.animator.SetTrigger("start");
        }

        public void OnEffectAnimationEnd() 
        {
            this.spriteRenderer.enabled = false;
            switch (this.effectType)
            {
                case RuneEffectTypes.EffectOnPlayer:
                    this.applyOnPlayer.Invoke(this.effectOnPlayer);
                    break;
                case RuneEffectTypes.EffectOnEnemy:
                    // waiting for any implemented enemies
                    break;
            }

            this.effectApplied++;
        }

        private void OnValidate()
        {
            if (this.effectType == RuneEffectTypes.ThrowProjectile && this.projectile == null) 
            {
                Debug.LogError(this.gameObject.name + " GameObject has no assigned projectile.");
            }

            if (this.TryGetComponent(out Animator anim)) 
            {
                this.animator = anim;
                if (this.animator.runtimeAnimatorController == null) 
                {
                    Debug.LogError("Animator component of " + this.gameObject.name + " GameObject does not have an Animator Controller assigned.");
                    return;
                }

                AnimatorController animController = this.animator.runtimeAnimatorController as AnimatorController;
                if (new List<AnimatorControllerParameter>(animController.parameters).Where(p => p.name == "start").Count() == 0) 
                {
                    Debug.LogError("Assigned Animator Controller on Animator component of " + this.gameObject.name + " GameObject does not have a \"start\" trigger parameter, which is necessary for the rune effect. Create the parameter to fix this issue.");
                }

                if (animController.animationClips == null || animController.animationClips.Length == 0)
                {
                    Debug.LogError("Assigned Animator Controller on Animator component of " + this.gameObject.name + " GameObject does not have an animation clip, which is necessary for the rune effect. Add an animation clip to fix this issue.");
                }
            }
            else 
            {
                this.animator = this.gameObject.AddComponent<Animator>();
                Debug.Log("Added Animator component for the animation that will be triggered once the rune effect is applied.");
            }

            if (!this.TryGetComponent(out SpriteRenderer spr)) 
            {
                this.gameObject.AddComponent<SpriteRenderer>();
                Debug.Log("Added SpriteRenderer component so animations are supported.");
            }
        }

    }
}
