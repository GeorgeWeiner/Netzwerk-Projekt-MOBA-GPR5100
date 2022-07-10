using UnityEngine;

namespace S_Abilities
{
    [CreateAssetMenu(menuName = "Sub Abilities", fileName = "New Sub Ability")]
    public abstract class SubAbility : ScriptableObject
    {
        //These can be used to re-use common functionality among different abilities.
        //For example any ability that can teleport someone can reuse its sub ability
        //for quick implementation of a teleport.
        
        public float subAbilityDelay;
        public float range;
        
        protected Transform TransformSelf;
        protected AbilityHandler AbilityHandler;
        
        public abstract void ExecuteSubAbility();

        public void InitializeSelf(Transform self)
        {
            TransformSelf = self;
        }
        
        public void InitializeSelf(Transform self, AbilityHandler handler)
        {
            TransformSelf = self;
            AbilityHandler = handler;
        }
    }
}