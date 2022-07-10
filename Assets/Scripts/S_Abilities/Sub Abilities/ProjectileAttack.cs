using UnityEngine;

namespace S_Abilities
{
    [CreateAssetMenu(menuName = "Sub Abilities/Projectile Attack", fileName = "New Projectile Attack")]
    public class ProjectileAttack : SubAbility
    {
        [SerializeField] private GameObject projectile;
        public override void ExecuteSubAbility()
        {
            AbilityHandler.SetPrefab(projectile);
        }
    }
}