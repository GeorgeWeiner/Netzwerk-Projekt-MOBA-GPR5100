using System;
using Interfaces;
using Mirror;
using UnityEngine;

namespace S_Combat
{
    public class Health : Stat, IDamageable
    {
        public event Action ServerOnDie;
        #region Server

        [Server]
        public void TakeDmg(int dmg)
        {
            if (currentValue == 0) return;
            currentValue = currentValue = Mathf.Max(currentValue- dmg, 0);

            if (currentValue != 0) return;

            ServerOnDie?.Invoke();

            Debug.Log("We died.");
        }

        #endregion

    }
}
