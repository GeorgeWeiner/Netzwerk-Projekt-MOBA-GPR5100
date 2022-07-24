using System.Collections.Generic;
using Interfaces;
using Mirror;
using S_Combat;
using UnityEngine;

namespace S_Abilities
{
    [CreateAssetMenu(menuName = "Sub Abilities/Box Overlap", fileName = "New Box Overlap")]
    public class GetOverlapBoxTargets : SubAbility, IListGenerator
    {
        [SerializeField] private float boxWidth, boxHeight, boxLength;
        [SerializeField] private bool hostileSpell;
        [SerializeField] private bool canTargetSelf;

        public List<Health> GetList<T>()
        {
            var targets = new List<Health>();
            
            var center = TransformSelf.position + TransformSelf.forward * boxLength;
            var halfExtends = new Vector3(boxWidth, boxHeight, boxLength);
            var colliders = Physics.OverlapBox(center, halfExtends, TransformSelf.rotation);

            foreach (var col in colliders)
            {
                if (col.TryGetComponent(out Health health))
                {
                    if (!canTargetSelf && col.gameObject == TransformSelf.gameObject) continue;
                    if (hostileSpell)
                    {
                        var targetTeam = health.netIdentity.GetComponent<MobaPlayerData>().team;
                        var selfTeam = NetworkClient.connection.identity.GetComponent<MobaPlayerData>().team;
                        if (targetTeam == selfTeam) continue;
                    }
                    
                    targets.Add(health);
                }
            }

            return targets;
        }
    }
}