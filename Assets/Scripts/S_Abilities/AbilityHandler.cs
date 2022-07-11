using System.Collections;
using Mirror;
using S_Combat;
using S_Manager;
using UnityEngine;

namespace S_Abilities
{
    public class AbilityHandler : NetworkBehaviour
    {
        //TODO: Replace this stuff with the actual logic of key bound abilities.
        [SerializeField] private Ability ability;
        [SerializeField] private Transform abilitySpawnOrigin;
        [SerializeField] private Mana mana;

        private void Update()
        {
            if (InputManager.instance.PressedAbilityButton())
            {
                CmdExecuteAbility();
            }
        }

        [Command]
        private void CmdExecuteAbility()
        {
            mana.UseMana(ability.manaCost, out var canUse);
            if (!canUse) return;
            
            StartCoroutine(ExecuteAbilitySteps());
        }
        
        private IEnumerator ExecuteAbilitySteps()
        {
            while (ability.AbilityQueue.TryDequeue(out var subAbility))
            {
                subAbility.InitializeSelf(transform, this);
                
                //TODO: Make this more extendable by following open-closed principle.
                if (subAbility.GetType() == typeof(ProjectileAttack))
                {
                    var projectileAttack = (ProjectileAttack) subAbility;
                    projectileAttack.InitializeSelf
                        (transform, this, abilitySpawnOrigin);
                }
                
                //TODO: If any termination conditions, such as a stun should occur
                //TODO: in the middle of casting, break the loop.

                subAbility.ExecuteSubAbility();
                yield return new WaitForSeconds(subAbility.subAbilityDelay);
            }
            
            ability.EnqueueSubAbilities();
            
            Debug.Log($"Finished executing {ability.abilityName}.");
        }

        public static void SetPrefab(GameObject prefab)
        {
            //TODO: This doesn't actually work, in that the prefabs are not spawnable...
            if (!NetworkClient.prefabs.ContainsValue(prefab))
            {
                NetworkClient.RegisterPrefab(prefab);
                print($"Registered prefab: {prefab.name}.");
            }
            
            GameObject match = null;
            if (!NetworkManager.singleton.spawnPrefabs.Contains(prefab)) return;
            match = NetworkManager.singleton.spawnPrefabs.Find(_ => prefab);
            
            SpawnPrefab(match);
        }
        
        public static void SetPrefab(GameObject prefab, Transform spawnPoint)
        {
            //TODO: This doesn't actually work, in that the prefabs are not spawnable...
            if (!NetworkClient.prefabs.ContainsValue(prefab))
            {
                NetworkClient.RegisterPrefab(prefab);
                print($"Registered prefab: {prefab.name}.");
            }
            
            GameObject match = null;
            if (!NetworkManager.singleton.spawnPrefabs.Contains(prefab)) return;
            match = NetworkManager.singleton.spawnPrefabs.Find(_ => prefab);
            
            SpawnPrefab(match, spawnPoint);
        }

        private static void SpawnPrefab(GameObject prefab)
        {
            MobaNetworkRoomManager.SpawnPrefab(prefab);
        }
        
        private static void SpawnPrefab(GameObject prefab, Transform spawnPoint)
        {
            MobaNetworkRoomManager.SpawnPrefab(prefab);
        }
    }
}