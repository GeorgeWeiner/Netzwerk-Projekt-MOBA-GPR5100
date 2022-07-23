using System.Collections.Generic;
using System.Linq;
using Interfaces;
using S_Combat;
using UnityEngine;

namespace S_Abilities
{
    [CreateAssetMenu(menuName = "Sub Abilities/Damage Units in Collection.", fileName = "New Damage Collection.")]
    public class DamageUnitsInList : SubAbility
    {
        [SerializeField] private int damageAmount;

        private Ability _ability;

        public override void ExecuteSubAbility()
        {
            /*For this to work, a sub-ability of type ListGenerator needs to be placed in front of this sub-ability in the inspector.
             It can be placed at any previous position before this one.*/
            
            var listGenerator = AbilityInstance.subAbilities.OfType<IListGenerator>().FirstOrDefault();
            List<Health> targets = listGenerator?.GetList<Health>();

            if (targets == null)
            {
                Debug.Log($"Targets was null. Returning.");
                return;
            }
            foreach (var health in targets)
            {
                Debug.Log($"Dealt damage to {health.gameObject.name}.");
                health.TakeDmg(damageAmount);
            }
        }
    }
}