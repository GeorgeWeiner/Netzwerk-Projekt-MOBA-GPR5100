using System;
using Mirror;
using S_Combat;
using S_Player;
using UnityEngine;
using UnityEngine.Events;

namespace S_Unit
{
    public class Unit : NetworkBehaviour
    {
        [SerializeField] private PlayerCommands _unitCommands;
        [SerializeField] private Targeter targeter;
        public Targeter Targeter{ get => targeter; set => targeter = value; }
        [SerializeField] private Health health;
        [SerializeField] private UnityEvent onSelected;
        [SerializeField] private UnityEvent onDeselected;
        
        public static event Action<Unit> ServerOnUnitSpawned; 
        public static event Action<Unit> ServerOnUnitDespawned;

        public static event Action<Unit> AuthorityOnUnitSpawned; 
        public static event Action<Unit> AuthorityOnUnitDespawned;
        
        
        public PlayerCommands GetUnitMovement()
        {
            return _unitCommands;
        }

        public override void OnStartServer()
        {
            ServerOnUnitSpawned?.Invoke(this);
            health.ServerOnDie += ServerHandleDie;
        }

        public override void OnStopServer()
        {
            ServerOnUnitDespawned?.Invoke(this);
            health.ServerOnDie -= ServerHandleDie;
        }

        [Server]
        private void ServerHandleDie()
        {
            NetworkServer.Destroy(gameObject);
        }
        

        public override void OnStartClient()
        {
            if (!isClientOnly) return;
            if (!hasAuthority) return;
            AuthorityOnUnitSpawned?.Invoke(this);
        }

        public override void OnStopClient()
        {
            if (!isClientOnly) return;
            if (!hasAuthority) return;
            AuthorityOnUnitDespawned?.Invoke(this);
        }

    }
}