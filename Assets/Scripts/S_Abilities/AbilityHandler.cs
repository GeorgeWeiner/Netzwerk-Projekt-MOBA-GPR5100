using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Combat;
using S_Player;
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
        [SerializeField] private Mana mana;

        private readonly Dictionary<AbilitySlot, Ability> _abilitySlots = new();
        private PlayerCommands _playerCommands;

        public event Action<SubAbility> SubAbilityExecuted;
        private Coroutine currentAbility;

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
            _playerCommands = GetComponent<PlayerCommands>();
            
            for (var i = 0; i < abilities.Count; i++)
            {
                foreach (var subAbility in abilities[i].subAbilities)
                {
                    subAbility.InitializeSelf(transform, this, abilities[i], _playerCommands);
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

            if (currentAbility != null)
            {
                StopCoroutine(currentAbility);
            }

            currentAbility = StartCoroutine(ExecuteAbilitySteps(_abilitySlots[slot]));
        }
        
        private IEnumerator ExecuteAbilitySteps(Ability ability)
        {
            //If transform position is not in range of the target walk towards it.
            //If the rotation is not facing the target rotate towards it.
            
            var target = _playerCommands.targeter.GetTarget();
            var targetPos = target == null ? _playerCommands.agent.destination : target.transform.position;
            
            float distance = Vector3.Distance(transform.position, targetPos);

            if (distance > ability.range && target != null)
            {
                _playerCommands.CmdMoveTowardsAttackTarget();
            }

            while (Vector3.Distance(transform.position, targetPos) > ability.range)
            {
                print("Waiting for the distance to target to shrink.");
                yield return null;
            }

            const float angle = 10f;
            var position = transform.position;

            while (Vector3.SignedAngle(transform.forward, position - targetPos, Vector3.up) > angle)
            {
                yield return null;
            }
            
            mana.UseMana(ability.manaCost, out bool canUse);
            if (!canUse)
            {
                print("Not enough mana left.");
                yield break;
            }
            
            Debug.Log("In position. Initiating Ability.");

            foreach (var subAbility in ability.subAbilities)
            {
                //subAbility.InitializeSelf(transform, this, ability);
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