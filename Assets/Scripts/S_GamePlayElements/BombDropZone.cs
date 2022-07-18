using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BombDropZone : NetworkBehaviour
{
    [SerializeField] Team teamSide;
    [SerializeField] float timeForBombToExplode;
    private void Start()
    {
        gameObject.DrawRectangle(GetComponent<LineRenderer>(), GetComponent<BoxCollider>().bounds.extents * 2);
    }
    void OnTriggerEnter(Collider other)
    {
        bool isRightSide = NetworkClient.connection.identity.TryGetComponent<MobaPlayerData>(out MobaPlayerData playerEnteringZone);
        bool isBombCarrier = other.TryGetComponent<BombCarrier>(out BombCarrier bombcarrier);
       
        if (isRightSide && isBombCarrier)
        {
            var bomb = bombcarrier.carriedBomb;
            
            if (teamSide != playerEnteringZone.team && bomb != null )
            {
                bomb.OnEnteringDropZone(timeForBombToExplode,playerEnteringZone.team);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        bool isRightSide = NetworkClient.connection.identity.TryGetComponent<MobaPlayerData>(out MobaPlayerData playerEnteringZone);
        bool isBombCarrier = other.TryGetComponent<BombCarrier>(out BombCarrier bombcarrier);
        
        if (isRightSide && isBombCarrier)
        {
            var bomb = bombcarrier.carriedBomb;
 
            if (teamSide != playerEnteringZone.team && bomb != null)
            {
                Debug.Log("STopped");
                bomb.OnPlayerExitDropZone();
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().bounds.extents * 2);
    }
}
