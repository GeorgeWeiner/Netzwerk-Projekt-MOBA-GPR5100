using System;
using Interfaces;
using Mirror;
using UnityEngine;

namespace S_Combat
{
    public class Health : NetworkBehaviour, IDamageable, ICharacterStat
    {
        [SerializeField] private int maxHealth = 100;

        [SyncVar(hook = nameof(HandleHealthUpdated))]
        [SerializeField] private int currentHealth;

        public event Action ServerOnDie;
        public event Action<int, int> ClientOnStatUpdated;

        #region Server

        public override void OnStartServer()
        {
            currentHealth = maxHealth;
        }
        
        [Server]
        public void TakeDmg(int dmg)
        {
            if (currentHealth == 0) return;
            currentHealth = currentHealth = Mathf.Max(currentHealth - dmg, 0);

            if (currentHealth != 0) return;

            ServerOnDie?.Invoke();

            Debug.Log("We died.");
        }

        #endregion

        #region Client

        private void HandleHealthUpdated(int oldHealth, int newHealth)
        {
            ClientOnStatUpdated?.Invoke(newHealth, maxHealth);
        }

        #endregion

        
    }
}
