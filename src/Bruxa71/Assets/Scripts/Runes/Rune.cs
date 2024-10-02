using Root.Player;
using Root.EditorExtensions.PropertyDrawers;
using Root.Projectiles;
using UnityEngine;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.Linq;

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

        public virtual void Activate(PlayerRunesManager player, Vector2 playerLookingDir)
        {
            if (this.inCooldown)
            {
                return;
            }

            switch (this.effectType)
            {
                case RuneEffectTypes.EffectOnPlayer:
                case RuneEffectTypes.EffectOnEnemy:
                    AnimationClip anim = this.animator.runtimeAnimatorController.animationClips[0];
                    AnimationEvent animEvent = new AnimationEvent();
                    animEvent.functionName = "OnEffectAnimationEnd";
                    animEvent.objectReferenceParameter = player;
                    animEvent.time = anim.length;
                    anim.AddEvent(animEvent);
                    this.animator.SetTrigger("start");
                    break;
                
                case RuneEffectTypes.ThrowProjectile:
                    Projectile proj = Instantiate(this.projectile, player.transform.position, Quaternion.identity);
                    proj.SetDirection(playerLookingDir);
                    this.animator.SetTrigger("start");
                    break;
            }
        }

        public void OnEffectAnimationEnd(PlayerRunesManager player) 
        {
            switch (this.effectType)
            {
                case RuneEffectTypes.EffectOnPlayer:
                    player.AddStatus(this.effectOnPlayer);
                    break;
                case RuneEffectTypes.EffectOnEnemy:
                    
                    break;
            }
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
        }

    }
}
