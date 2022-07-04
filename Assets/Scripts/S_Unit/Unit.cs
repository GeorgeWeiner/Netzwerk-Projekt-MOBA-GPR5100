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
        [SerializeField] private PlayerMovement unitMovement;
        [SerializeField] private Targeter targeter;
        [SerializeField] private Health health;
        [SerializeField] private UnityEvent onSelected;
        [SerializeField] private UnityEvent onDeselected;
        
        public static event Action<Unit> ServerOnUnitSpawned; 
        public static event Action<Unit> ServerOnUnitDespawned;

        public static event Action<Unit> AuthorityOnUnitSpawned; 
        public static event Action<Unit> AuthorityOnUnitDespawned;
        
        public PlayerMovement GetUnitMovement()
        {
            return unitMovement;
        }
    }
}