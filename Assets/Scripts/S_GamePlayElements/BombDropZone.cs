using Mirror;
using S_Extensions;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BombDropZone : NetworkBehaviour
{
    [SerializeField] private Team teamSide;
    [SerializeField] private float timeForBombToExplode;
    private void Start()
    {
        gameObject.DrawRectangle(GetComponent<LineRenderer>(), GetComponent<BoxCollider>().bounds.extents * 2);
    }
    /// <summary>
    /// Defines what happens when a bomb carrier carries the bomb
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        bool isRightSide = NetworkClient.connection.identity.TryGetComponent<MobaPlayerData>(out MobaPlayerData playerEnteringZone);
        bool isBombCarrier = other.TryGetComponent<BombCarrier>(out BombCarrier bombcarrier);
       //if its a player and a bombcarrier continue
        if (isRightSide && isBombCarrier)
        {
            var bomb = bombcarrier.carriedBomb;
            
            if (teamSide != playerEnteringZone.team && bomb != null )
            {
                bomb.OnEnteringDropZone(timeForBombToExplode,playerEnteringZone.team);
            }
        }
    }
    /// <summary>
    /// defines whats happening when somebody leaves the drop zon
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        bool isRightSide = NetworkClient.connection.identity.TryGetComponent<MobaPlayerData>(out MobaPlayerData playerEnteringZone);
        bool isBombCarrier = other.TryGetComponent<BombCarrier>(out BombCarrier bombcarrier);
        
        if (isRightSide && isBombCarrier)
        {
            var bomb = bombcarrier.carriedBomb;
            //Callback to the bomb when you exit the drop zone
            if (teamSide != playerEnteringZone.team && bomb != null)
            {
                bomb.OnPlayerExitDropZone();
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().bounds.extents * 2);
    }
}
