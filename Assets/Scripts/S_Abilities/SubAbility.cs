using UnityEngine;
using UnityEngine.VFX;

namespace S_Abilities
{
    [CreateAssetMenu(menuName = "Sub Abilities", fileName = "New Sub Ability")]
    public abstract class SubAbility : ScriptableObject
    {
        //These can be used to re-use common functionality among different abilities.
        //For example any ability that can teleport someone can reuse its sub ability
        //for quick implementation of a teleport.
        
        [Header("Sub Ability Stats")]
        [Range(0f, 10f)] public float subAbilityDelay;
        public bool isStationary, isCancellable;

        protected Transform TransformSelf;
        protected AbilityHandler abilityHandler;
        protected Ability AbilityInstance;

        [Header("Visual Effects")]
        public VisualEffectAsset visualEffectAsset;
        [Range(0.01f, 5f)] public float visualEffectPlaybackSpeed = 1f;
        [Range(0.1f, 10f)] public float visualEffectDurationInSeconds = 1f;

        public abstract void ExecuteSubAbility();

        //Inject dependencies.
        public void InitializeSelf(Transform self, AbilityHandler handler, Ability ability)
        {
            TransformSelf = self;
            abilityHandler = handler;
            AbilityInstance = ability;
        }
    }
}