using Mirror;
using UnityEngine;

namespace S_Combat
{
    public class Targetable : NetworkBehaviour
    {
        [SerializeField] private Transform aimAtPoint;
        [SyncVar(hook = nameof(ChangeTeam))]Team currentTeam;
        public Team CurrentTeam { get => currentTeam; set => currentTeam = value; }

        public Transform GetAimAtPoint()
        {
            return aimAtPoint;
        }

        void ChangeTeam(Team old, Team newTeam)
        {
            currentTeam = newTeam;
        }
    }
}