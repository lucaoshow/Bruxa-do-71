using Root.EditorExtensions.PropertyDrawers;
using UnityEngine;
using Root.Projectiles;
using Root.Player;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.Linq;

namespace Root.Runes
{
    public class RuneEffect : MonoBehaviour
    {
        [SerializeField] private RuneEffectTypes effectType;

        [DrawIf("effectType", RuneEffectTypes.EffectOnPlayer)]
        [SerializeField] private EffectOnPlayer effectOnPlayer;

        [DrawIf("effectType", RuneEffectTypes.EffectOnEnemy)]
        [SerializeField] private EffectOnEnemy effectOnEnemy;

        [DrawIf("effectType", RuneEffectTypes.ThrowProjectile)]
        [SerializeField] private Projectile projectile;

        private Animator animator;

        public void Apply(PlayerRunesManager player, Vector2 direction) 
        {
            switch (this.effectType)
            {
                case RuneEffectTypes.EffectOnPlayer:
                case RuneEffectTypes.EffectOnEnemy:
                    AnimationClip anim = this.animator.runtimeAnimatorController.animationClips[0];
                    AnimationEvent animEvent = new AnimationEvent();
                    animEvent.functionName = "OnEffectAnimationEnd";
                    animEvent.time = anim.length;
                    anim.AddEvent(animEvent);
                    this.animator.SetTrigger("start");
                    break;
                
                case RuneEffectTypes.ThrowProjectile:
                    Projectile proj = Instantiate(this.projectile, player.transform.position, Quaternion.identity, player.transform);
                    proj.SetDirection(direction);
                    break;
            }
        }

        public void OnEffectAnimationEnd() 
        {
            
        }

        private void OnValidate() {
            if (EditorApplication.isPlaying) 
            {
                return;
            }

            if (this.animator != null) 
            {
                if (this.effectType == RuneEffectTypes.ThrowProjectile) 
                { 
                    Destroy(this.animator);
                    this.animator = null;
                }

                if (this.animator.runtimeAnimatorController == null) 
                {
                    return;
                }

                AnimatorController animController = this.animator.runtimeAnimatorController as AnimatorController;
                if (new List<AnimatorControllerParameter>(animController.parameters).Where(p => p.name == "start").Count() == 0) 
                {
                    Debug.LogError("Assigned Animator Controller on Animator component of " + this.gameObject.name + " GameObject does not have a \"start\" trigger parameter, which is necessary for the Rune Effect. Create the parameter to fix this issue.");
                }
            }
            else if (this.effectType != RuneEffectTypes.ThrowProjectile) 
            {
                this.animator = this.gameObject.AddComponent<Animator>();
                Debug.Log("Added Animator component for the animation that will be triggered once the Rune Effect is applied. When the animation ends, the effect is applied.");
            }
        }
    }
}
