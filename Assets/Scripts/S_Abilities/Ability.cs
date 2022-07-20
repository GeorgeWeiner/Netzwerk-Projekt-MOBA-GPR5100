using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace S_Abilities
{
    [CreateAssetMenu(menuName = "Ability", fileName = "New Ability")]
    public sealed class Ability : ScriptableObject
    {
        //Base class for ability system.
        //Create a Queue of sub-abilities and loop through them.

        public Sprite thumbNail;
        public string abilityName;
        public int manaCost;
        public float coolDown;
        
        public List<SubAbility> subAbilities;

        public VisualEffect visualEffect;

        public List<SubAbility> instancedSubAbilities = new();
        
        private void OnEnable()
        {
            //CreateInstances();
        }

        //private void CreateInstances()
        //{
        //    foreach (var subAbility in subAbilities)
        //    {
        //        var instance = Instantiate(subAbility);
        //        instancedSubAbilities.Add(instance);
        //    }
        //}
    }
}