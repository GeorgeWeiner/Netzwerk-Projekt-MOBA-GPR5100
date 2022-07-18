using System;
using Interfaces;
using Mirror;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace S_Combat
{
    public class Mana : Stat, ICharacterStat
    {
        public override void OnStartClient()
        {
            GameManager.Instance.OnPlayerDie += OnRevive;
            GameManager.Instance.OnRoundWon += OnRoundReset;
        }
        public void UseMana(int amount, out bool canUse)
        {
            canUse = currentValue - amount > 0;
            if (!canUse) return;
            
            currentValue -= amount;
        }
        void OnDestroy()
        {
            GameManager.Instance.OnPlayerDie -= OnRevive;
            GameManager.Instance.OnRoundWon -= OnRoundReset;
        }
        void OnRevive(MobaPlayerData player)
        {
            currentValue = MaxValue;
        }
        void OnRoundReset()
        {
            currentValue = MaxValue;
        }
    }
}