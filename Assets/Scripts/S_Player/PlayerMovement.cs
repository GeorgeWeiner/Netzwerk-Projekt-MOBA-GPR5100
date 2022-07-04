using Mirror;
using S_Combat;
using UnityEngine;
using UnityEngine.AI;

namespace S_Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        [SerializeField] private Targeter targeter;
        [SerializeField] private float chaseRange = 10f;
        
        private NavMeshAgent agent;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }
        
        //Moved all of this into the UnitCommandGiver.cs to make to code more flexible.
        
        //[Client]
        //private void Update()
        //{
        //    if (InputManager.instance.ClickedRightMouseButton())
        //    {
        //        SetMovePoint();
        //    }
        //}
        
        //[Client]
        //private void SetMovePoint()
        //{
        //    if (!hasAuthority) return;
        //    
        //    Ray ray = mainCamera.ScreenPointToRay(InputManager.instance.GetMousePos());
        //    
        //    bool validPos = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer);
        //    if (!validPos) return;
        //    
        //    CmdMove(hit.point);
        //}

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
