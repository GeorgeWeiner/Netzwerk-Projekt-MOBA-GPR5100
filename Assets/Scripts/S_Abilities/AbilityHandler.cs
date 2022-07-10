using System;
using System.Collections;
using Mirror;
using UnityEngine;

namespace S_Abilities
{
    public class AbilityHandler : NetworkBehaviour
    {
        [SerializeField]
        private Ability ability;

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
            
            CmdSpawnPrefab(find);
        }

        private void CmdSpawnPrefab(GameObject prefab)
        {
            MobaNetworkRoomManager.SpawnPrefab(prefab);

            //var outputPrefab = Instantiate(prefab);
            //NetworkServer.Spawn(outputPrefab, connectionToClient);
        }
    }
}