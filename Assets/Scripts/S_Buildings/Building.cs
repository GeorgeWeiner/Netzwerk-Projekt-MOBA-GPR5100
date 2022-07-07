using System;
using Mirror;
using UnityEngine;

namespace S_Buildings
{
    public class Building : NetworkBehaviour
    {
        [SerializeField] protected Health health;
        
        public static event Action<Building> ServerOnBuildingSpawned; 
        public static event Action<Building> ServerOnBuildingDespawned;
        
        public override void OnStartServer()
        {
            ServerOnBuildingSpawned?.Invoke(this);
            health.ServerOnDie += ServerHandleDie;
        }

        public override void OnStopServer()
        {
            ServerOnBuildingDespawned?.Invoke(this);
            health.ServerOnDie -= ServerHandleDie;
        }
        
        [Server]
        protected virtual void ServerHandleDie()
        {
            NetworkServer.Destroy(gameObject);
        }
    }
}