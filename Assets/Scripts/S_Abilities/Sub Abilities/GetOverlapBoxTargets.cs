using System.Collections.Generic;
using System.Linq;
using Interfaces;
using S_Combat;
using UnityEngine;

namespace S_Abilities
{
    [CreateAssetMenu(menuName = "Sub Abilities/Box Overlap", fileName = "New Box Overlap")]
    public class GetOverlapBoxTargets : SubAbility, IListGenerator
    {
        [HideInInspector]
        public List<Health> _targets = new();
        
        [SerializeField] private float boxWidth, boxHeight, boxLength;
        [SerializeField] private bool hostileSpell;
        [SerializeField] private bool canTargetSelf;
        
        public override void ExecuteSubAbility()
        {
            //Add extra functionality here if you please.
        }

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
                    
                    targets.Add(health);
                    Debug.Log($"Found health! {health.gameObject.name}");
                }
            }
            
            Debug.Log("Targets Length:" + targets.Count);

            return targets;
        }
    }
}