using Mirror;
using S_Combat;
using UnityEngine;
using UnityEngine.AI;

namespace S_Player
{
    public class PlayerCommands : NetworkBehaviour
    {
        [SerializeField] private Targeter targeter;
        [SerializeField] private float chaseRange = 10f;
        
        private NavMeshAgent agent;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }
        [Command]
        public void CmdMove(Vector3 position)
        {
            targeter.ClearTarget();
            
            bool validPosition = 
                NavMesh.SamplePosition(position, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas);
            
            if (!validPosition) return;
            
            agent.SetDestination(hit.position);
        }
    }
}
