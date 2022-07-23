using Mirror;
using S_Combat;
using TMPro;
using UnityEngine;

/// <summary>
/// The class which handles bomb pulling picking it up 
/// </summary>
public class BombCarrier : NetworkBehaviour
{
    [SerializeField] private LayerMask bombLayer;
    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private float pickUpRadius;
    [SerializeField] private float bombCarrySpeed;
    [SyncVar(hook = nameof(UpdateBombState))]public  Bomb carriedBomb;
    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();
        health.ServerOnDie += CmdDropBomb;
    }

    private void Update()
    {
        TryPickUpBomb();
        if (hasAuthority && carriedBomb != null)
        {
            CmdCarryBomb();
        }
        if (health.CurrentValue <= 0 && hasAuthority && !health.IsDead)
        {
            CmdDropBomb();
        }
    }
    /// <summary>
    /// Checks if you can pick up the bomb and if you press the right key if you can pick it up
    /// </summary>
    private void TryPickUpBomb()
    {
        if (!CanPickUpBomb()) return;
        {
            if (!InputManager.instance.PressedInteractionKey() || !hasAuthority) return;
            {
                CmdPickUpBomb();
            }
        }
    }
    /// <summary>
    /// Function which returns if the bomb is pickupable and activates and deactivates the indicator for it
    /// </summary>
    /// <returns></returns>
    private bool CanPickUpBomb()
    {
        if (Physics.CheckSphere(transform.position,pickUpRadius, bombLayer) && hasAuthority && carriedBomb == null)
        {
            interactionText.gameObject.SetActive(true);
            return true;
        }

        if (interactionText != null)
        {
            if (interactionText.gameObject.activeSelf && hasAuthority)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,pickUpRadius);
    }

    #region ClientCommands
    /// <summary>
    /// A command for updating the bombs position
    /// </summary>
    [Command]
    private void CmdCarryBomb()
    {
        RpcMoveBomb();
    }
    [Command]
    private void CmdDropBomb()
    {
        if (carriedBomb != null)
        {
            carriedBomb.OnDrop();
            carriedBomb = null;
        }
    }
    /// <summary>
    /// A command that gets called if the bomb gets picked up
    /// </summary>
    [Command]
    private void CmdPickUpBomb()
    {
        Collider[] bomb = Physics.OverlapSphere(transform.position, pickUpRadius, bombLayer);

        foreach (var collider1 in bomb)
        {
            if (!collider1.gameObject.TryGetComponent<Bomb>(out Bomb bomb1)) return;
            {
                if (bomb1.IsPickedUp || bomb1.IsGettingPickedUp) return;
                {
                    bomb1.OnPickUp(this);
                }
            }
        }
    }
    #endregion

    #region Rpcs
    /// <summary>
    /// Rpc for moving the bomb which gets called by the carry bomb command
    /// </summary>
    [ClientRpc]
    private void RpcMoveBomb()
    {
        if (carriedBomb != null && carriedBomb.IsPickedUp)
        {
            carriedBomb.transform.position = Vector3.MoveTowards(carriedBomb.transform.position,
                transform.position - transform.forward, Time.deltaTime * bombCarrySpeed);
        }
    }

    #endregion

    #region Hooks

    private void UpdateBombState(Bomb old,Bomb newBomb)
    {
        carriedBomb = newBomb;
    }

    #endregion

}
