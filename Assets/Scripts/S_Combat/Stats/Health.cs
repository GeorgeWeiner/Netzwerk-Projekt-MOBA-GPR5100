using System;
using Interfaces;
using Mirror;
using UnityEngine;

namespace S_Combat
{
    public class Health : Stat, IDamageable
    {
        public event Action ServerOnDie;
        bool isDead = false;
        public bool IsDead { get => isDead; }

        public override void OnStartClient()
        {
            GameManager.Instance.OnPlayerRevive += OnRevive;
            GameManager.Instance.OnRoundWon += OnRoundReset;
        }
        void OnDestroy()
        {
            GameManager.Instance.OnPlayerRevive -= OnRevive;
            GameManager.Instance.OnRoundWon -= OnRoundReset;
        }
        #region Server

        [Server]
        public void TakeDmg(int dmg)
        {
            if (currentValue == 0) return;
            currentValue = currentValue = Mathf.Max(currentValue- dmg, 0);

            if (currentValue != 0) return;
           
            ServerOnDie?.Invoke();
            GameManager.Instance.PlayerDiedCallback(playerDataForCallbacks);
            isDead = true;
        }

        void OnRevive(MobaPlayerData player)
        {
            isDead = false;
            currentValue = MaxValue;
        }
        void OnRoundReset()
        {
            isDead = false;
            currentValue = MaxValue;
        }
        #endregion

    }
}
