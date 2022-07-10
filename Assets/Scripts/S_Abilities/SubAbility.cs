using UnityEngine;

namespace S_Abilities
{
    [CreateAssetMenu(menuName = "Sub Abilities", fileName = "New Sub Ability")]
    public abstract class SubAbility : ScriptableObject
    {
        //These can be used to reuse common functionality among different abilities.
        //For example any ability that can teleport someone can reuse its sub ability for quick reuse.
        
        public float subAbilityDelay;
        public float range;
        
        protected Transform transformSelf;
        protected AbilityHandler AbilityHandler;
        
        public abstract void ExecuteSubAbility();

        public void InitializeSelf(Transform self)
        {
            transformSelf = self;
        }
        
        public void InitializeSelf(Transform self, AbilityHandler handler)
        {
            transformSelf = self;
            AbilityHandler = handler;
        }
    }
}