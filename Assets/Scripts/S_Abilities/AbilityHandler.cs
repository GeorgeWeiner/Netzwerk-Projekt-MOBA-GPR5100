using System;
using System.Collections;
using Mirror;
using S_Manager;
using UnityEngine;

namespace S_Abilities
{
    public class AbilityHandler : NetworkBehaviour
    {
        [SerializeField] private Ability ability;
        [SerializeField] private Transform abilitySpawnOrigin;

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
            StartCoroutine(ExecuteAbilitySteps());
        }
        
        private IEnumerator ExecuteAbilitySteps()
        {
            while (ability.AbilityQueue.TryDequeue(out var subAbility))
            {
                subAbility.InitializeSelf(transform, this);
                if (subAbility.GetType() == typeof(ProjectileAttack))
                {
                    var projectileAttack = (ProjectileAttack) subAbility;
                    projectileAttack.InitializeSelf
                        (transform, this, abilitySpawnOrigin);
                }
                
                subAbility.ExecuteSubAbility();
                yield return new WaitForSeconds(subAbility.subAbilityDelay);
            }
            
            ability.EnqueueSubAbilities();
            
            Debug.Log($"Finished executing {ability.abilityName}.");
        }

        public void SetPrefab(GameObject prefab)
        {
            if (!NetworkClient.prefabs.ContainsValue(prefab))
            {
                NetworkClient.RegisterPrefab(prefab);
                print($"Registered prefab: {prefab.name}.");
            }
            
            GameObject find = null;
            if (!NetworkManager.singleton.spawnPrefabs.Contains(prefab)) return;
            find = NetworkManager.singleton.spawnPrefabs.Find(_ => prefab);
            
            SpawnPrefab(find);
        }
        
        public void SetPrefab(GameObject prefab, Transform spawnPoint)
        {
            if (!NetworkClient.prefabs.ContainsValue(prefab))
            {
                NetworkClient.RegisterPrefab(prefab);
                print($"Registered prefab: {prefab.name}.");
            }
            
            GameObject find = null;
            if (!NetworkManager.singleton.spawnPrefabs.Contains(prefab)) return;
            find = NetworkManager.singleton.spawnPrefabs.Find(_ => prefab);
            
            SpawnPrefab(find, spawnPoint);
        }

        private void SpawnPrefab(GameObject prefab)
        {
            MobaNetworkManager.SpawnPrefab(prefab);

            //var outputPrefab = Instantiate(prefab);
            //NetworkServer.Spawn(outputPrefab, connectionToClient);
        }
        
        private void SpawnPrefab(GameObject prefab, Transform spawnPoint)
        {
            MobaNetworkManager.SpawnPrefab(prefab, spawnPoint);
        }
    }
}