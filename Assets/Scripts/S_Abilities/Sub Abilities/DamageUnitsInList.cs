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
            var listGenerator = AbilityInstance.instancedSubAbilities.OfType<IListGenerator>().FirstOrDefault();
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