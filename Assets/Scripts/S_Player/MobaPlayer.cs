using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Unit = S_Unit.Unit;

namespace S_Player
{
    //This is the class that is responsible for handling the units under your command.
    public class MobaPlayer : NetworkBehaviour
    {
        [SerializeField] private List<Unit> myUnits = new List<Unit>();

        private void Start()
        {
            NetworkClient.ready = true;
        }
        public List<Unit> GetMyUnits()
        {
            return myUnits;
        }

        public override void OnStartServer()
        {
            Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;
            Unit.ServerOnUnitDespawned += ServerHandleUnitDespawned;
        }

        private void ServerHandleUnitSpawned(Unit unit)
        {
            if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;
            //This can be null
            myUnits.Add(unit);
        }

        private void ServerHandleUnitDespawned(Unit unit)
        {
            if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;
            myUnits.Remove(unit);
        }

        public override void OnStartClient()
        {
            if (!hasAuthority) return;

            Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitSpawned;
            Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitDespawned;
        }

        public override void OnStopClient()
        {
            if (!hasAuthority) return;

            Unit.AuthorityOnUnitSpawned -= AuthorityHandleUnitSpawned;
            Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitDespawned;
        }

        private void AuthorityHandleUnitSpawned(Unit unit)
        {
            if (!hasAuthority) return;
            myUnits.Add(unit);
        }

        private void AuthorityHandleUnitDespawned(Unit unit)
        {
            if (!hasAuthority) return;
            myUnits.Remove(unit);
        }
        
    }
}