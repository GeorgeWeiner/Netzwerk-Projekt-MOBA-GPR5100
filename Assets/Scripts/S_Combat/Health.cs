using System;
using Mirror;
using UnityEngine;

public class Health : NetworkBehaviour,IDamageable
    {
        [SerializeField] private int maxHealth = 100;

        [SyncVar(hook = nameof(HandleHealthUpdated))]
        private int currentHealth;

        public event Action ServerOnDie;
        public event Action<int, int> ClientOnHealthUpdated; 

        #region Server

        public override void OnStartServer()
        {
            currentHealth = maxHealth;
        }
        [Server]
        public void TakeDmg(int dmg)
        {
            if (currentHealth == 0) return;
            Debug.Log("Attack22222");
            currentHealth = currentHealth = Mathf.Max(currentHealth - dmg, 0);

            if (currentHealth != 0) return;

            ServerOnDie?.Invoke();

            Debug.Log("We died.");
        }

    #endregion

        #region Client

    private void HandleHealthUpdated(int oldHealth, int newHealth)
        {
            ClientOnHealthUpdated?.Invoke(newHealth, maxHealth);
        }

        #endregion
    }
