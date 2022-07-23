using System.Collections.Generic;
using UnityEngine;

namespace S_Abilities
{
    [CreateAssetMenu(menuName = "Ability", fileName = "New Ability")]
    public sealed class Ability : ScriptableObject
    {
        /*Base class for ability system.
        This acts as a blueprint for sub-abilities.
        Any type of ability stats can also be saved here.*/

        public Sprite thumbNail;
        public string abilityName;
        public int manaCost;
        public float coolDown;
        
        public List<SubAbility> subAbilities;
    }
}