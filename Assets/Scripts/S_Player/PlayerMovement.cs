using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] LayerMask groundLayer;
    NavMeshAgent agent;
    Camera mainCamera;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
    }
    [Client]
    void Update()
    {
        if (InputManager.instance.ClickedRightMouseButton())
        {
            SetMovePoint();
        }
       
    }

    [Command]
    void MovePlayerTowardsPoint(Vector3 position)
    {
        bool validPosition = NavMesh.SamplePosition(position, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas);
        Debug.Log(position);
        if (!validPosition) return;
        {
            agent.SetDestination(hit.position);
        }
    }
    [Client]
    void SetMovePoint()
    {
        if (!hasAuthority) return;
        {
            Ray ray = mainCamera.ScreenPointToRay(InputManager.instance.GetMousePos());
            bool validPos = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer);

            if (!validPos) return;
            {
               MovePlayerTowardsPoint(hit.point);
            }
        }
    }
}
