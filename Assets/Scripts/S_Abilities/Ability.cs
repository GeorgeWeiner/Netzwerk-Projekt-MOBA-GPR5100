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
        public readonly Queue<SubAbility> AbilityQueue = new ();

        public VisualEffect visualEffect;

        private void OnEnable()
        {
            Initialize();
        }

        public void Initialize()
        {
            EnqueueSubAbilities();
        }

        private void EnqueueSubAbilities()
        {
            if (subAbilities.Count == 0)
            {
                Debug.LogWarning($"Ability {abilityName} has no sub abilities!.");
                return;
            }
            foreach (var subAbility in subAbilities)
            {
                var instance = Instantiate(subAbility);
                AbilityQueue.Enqueue(instance);
            }
        }
    }
}