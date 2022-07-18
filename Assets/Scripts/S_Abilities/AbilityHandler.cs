using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Combat;
using S_Manager;
using UnityEngine;

public enum AbilitySlot
{
    AbilityOne,
    AbilityTwo,
    AbilityThree,
    AbilityUltimate,
    AbilityFour,
    AbilityFive
}

namespace S_Abilities
{
    public class AbilityHandler : NetworkBehaviour
    {
        //TODO: Replace this stuff with the actual logic of key bound abilities.
        [SerializeField] private List<Ability> abilities;
        [SerializeField] private Transform abilitySpawnOrigin;
        [SerializeField] private Mana mana;

        private readonly Dictionary<AbilitySlot, Ability> _abilitySlots = new();
        public List<Ability> Abilities => abilities;
        public static event Action<SubAbility> SubAbilityExecuted;

        public override void OnStartServer()
        {
            InputManager.OnPressedAbility += AbilityCallback;

            for (var i = 0; i <= abilities.Count; i++)
            {
                //Create instances so the same hero can be used multiple times in a game.
                var abilityInstance = Instantiate(abilities[i]);
                foreach (var subAbility in abilityInstance.instancedSubAbilities)
                {
                    subAbility.InitializeSelf(transform, this, abilityInstance);
                }
                _abilitySlots.Add((AbilitySlot)i, abilityInstance);
            }
        }

        private void AbilityCallback(AbilitySlot slot)
        {
            if (!hasAuthority) return;
            CmdExecuteAbility(slot);
        }

        [Command]
        private void CmdExecuteAbility(AbilitySlot slot)
        {
            if (!_abilitySlots.ContainsKey(slot))
            {
                Debug.Log("No ability assigned to this slot. Returning.");
                return;
            }
            
            mana.UseMana(_abilitySlots[slot].manaCost, out var canUse);
            if (!canUse)
            {
                print("Not enough mana left.");
                return;
            }

            StartCoroutine(ExecuteAbilitySteps(_abilitySlots[slot]));
        }
        
        private IEnumerator ExecuteAbilitySteps(Ability ability)
        {
            foreach (var subAbility in ability.instancedSubAbilities)
            {
                subAbility.InitializeSelf(transform, this, ability);
                
                //TODO: Make this more extendable by following open-closed principle.
                if (subAbility.GetType() == typeof(ProjectileAttack))
                {
                    var projectileAttack = (ProjectileAttack) subAbility;
                    projectileAttack.InitializeSelf
                        (transform, this, abilitySpawnOrigin);
                }
                
                //TODO: If any termination conditions, such as a stun should occur in the middle of casting, break the loop.

                subAbility.ExecuteSubAbility();
                SubAbilityExecuted?.Invoke(subAbility);
                yield return new WaitForSeconds(subAbility.subAbilityDelay);
            }

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
            MobaNetworkRoomManager.SpawnPrefab(prefab,spawnPoint);
        }
    }
}