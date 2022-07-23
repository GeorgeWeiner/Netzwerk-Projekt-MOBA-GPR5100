using System;
using Mirror;
using UnityEngine;

namespace S_Combat
{
    /// <summary>
    /// Health class for champions minions etc
    /// </summary>
    public class Health : Stat, IDamageable
    {
        public event Action ServerOnDie;
        private bool isDead = false;
        public bool IsDead { get => isDead; }

        public override void OnStartClient()
        {
            GameManager.Instance.OnPlayerRevive += OnRevive;
            GameManager.Instance.OnRoundWon += OnRoundReset;
        }

        private void OnDestroy()
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

        private void OnRevive(MobaPlayerData player)
        {
            isDead = false;
            currentValue = MaxValue;
        }

        private void OnRoundReset()
        {
            isDead = false;
            currentValue = MaxValue;
        }
        #endregion

    }
}
