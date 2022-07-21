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
    }
}