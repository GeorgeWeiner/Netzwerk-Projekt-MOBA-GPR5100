using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Combat;
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
        [SerializeField] private List<Ability> abilities;
        [SerializeField] private Transform abilitySpawnOrigin;
        [SerializeField] private Mana mana;

        private readonly Dictionary<AbilitySlot, Ability> _abilitySlots = new();

        public event Action<SubAbility> SubAbilityExecuted;

        //public override void OnStartServer()
        //{
        //    InputManager.OnPressedAbility += AbilityCallback;
        //
        //    /*Initialize the sub-abilities with the necessary references via dependency-injection.
        //     Then, add the ability to the dictionary as a value, with the correlating ability-slot as key.*/
        //    for (var i = 0; i < abilities.Count; i++)
        //    {
        //        foreach (var subAbility in abilities[i].subAbilities)
        //        {
        //            subAbility.InitializeSelf(transform, this, abilities[i]);
        //        }
        //        _abilitySlots.Add((AbilitySlot)i, abilities[i]);
        //    }
        //}

        public override void OnStartClient()
        {
            InputManager.OnPressedAbility += AbilityCallback;
            
            for (var i = 0; i < abilities.Count; i++)
            {
                foreach (var subAbility in abilities[i].subAbilities)
                {
                    subAbility.InitializeSelf(transform, this, abilities[i]);
                }
                _abilitySlots.Add((AbilitySlot)i, abilities[i]);
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
            
            mana.UseMana(_abilitySlots[slot].manaCost, out bool canUse);
            if (!canUse)
            {
                print("Not enough mana left.");
                return;
            }

            StartCoroutine(ExecuteAbilitySteps(_abilitySlots[slot]));
        }
        
        private IEnumerator ExecuteAbilitySteps(Ability ability)
        {
            foreach (var subAbility in ability.subAbilities)
            {
                subAbility.InitializeSelf(transform, this, ability);
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