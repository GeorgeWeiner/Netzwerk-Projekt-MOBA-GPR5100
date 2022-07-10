using System.Runtime.Serialization;
using S_Combat;
using UnityEngine;

namespace S_Abilities
{
    [CreateAssetMenu(menuName = "Sub Abilities/Target Attack", fileName = "New Target Attack")]
    public class TargetAttack : SubAbility
    {
        [SerializeField] private int damage;
        private Targetable _target;

        public override void ExecuteSubAbility()
        {
            _target = TransformSelf.GetComponent<Targeter>().GetTarget();
            
            if (TransformSelf.position.GetSqredDistance(_target.transform.position) > range) return;
            if (!_target.TryGetComponent(out Health health)) return;
            health.TakeDmg(damage);
            Debug.Log($"Successfully executed TargetAttack.");
        }
    }
}