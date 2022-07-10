using System;
using Interfaces;
using Mirror;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace S_Combat
{
    public class Mana : NetworkBehaviour, ICharacterStat
    {
        [SerializeField] private int maxMana;
        
        [SyncVar(hook = nameof(HandleHealthUpdated))]
        private int _currentMana;
        
        public event Action<int, int> ClientOnStatUpdated;
        
        public override void OnStartServer()
        {
            _currentMana = maxMana;
        }

        public void UseMana(int amount, out bool canUse)
        {
            canUse = _currentMana - amount > 0;
            if (!canUse) return;
            
            _currentMana -= amount;
        }

        private void HandleHealthUpdated(int oldValue, int newValue) 
        {
            ClientOnStatUpdated?.Invoke(newValue, maxMana);
        }
    }
}