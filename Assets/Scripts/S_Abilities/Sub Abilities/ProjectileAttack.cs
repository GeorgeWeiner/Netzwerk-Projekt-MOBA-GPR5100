using UnityEngine;

namespace S_Abilities
{
    [CreateAssetMenu(menuName = "Sub Abilities/Projectile Attack", fileName = "New Projectile Attack")]
    public class ProjectileAttack : SubAbility
    {
        [SerializeField] private GameObject projectile;
        
        private Transform _spawnPoint;
        
        public override void ExecuteSubAbility()
        {
            AbilityHandler.SetPrefab(projectile, _spawnPoint);
        }

        public void InitializeSelf(Transform self, AbilityHandler handler, Transform projectileSpawnTransform)
        {
            TransformSelf = self;
            abilityHandler = handler;
            _spawnPoint = projectileSpawnTransform;
        }
    }
}