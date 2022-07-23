using System.Collections;
using Mirror;
using S_Extensions;
using UnityEngine.UI;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class Bomb : NetworkBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Image timer;
    [SerializeField] private Vector3 pickUpZoneSize;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float timeForPickup;

    [SyncVar(hook = nameof(UpdateTimer))] private float pickupTime;
    [SyncVar(hook = nameof(UpdateBombState))]
    private bool isGettingPickedUp;
    public bool IsGettingPickedUp{ get => isGettingPickedUp; }
    [SyncVar(hook = nameof(UpdateBombIsPickedUpState))]
    private bool isPickedUp = false;
    public bool IsPickedUp{ get => isPickedUp; }
    private Coroutine pickUpRoutine;
    private Coroutine explosionRoutine;

    private void Start()
    {
        gameObject.DrawRectangle(lineRenderer, pickUpZoneSize);
    }
    /// <summary>
    /// Rpc taht handles ehat happens when somebody picks up the bomb
    /// </summary>
    /// <param name="carrier"></param>
    [ClientRpc]
    public void OnPickUp(BombCarrier carrier)
    {
        if (pickUpRoutine != null) return;
        {
            lineRenderer.enabled = true;
            isGettingPickedUp = true;
            pickUpRoutine = StartCoroutine(PickUpTimer(carrier));
        }
    }
    /// <summary>
    /// Rpc that handles what happens if somebody drops the bomb
    /// </summary>
    [ClientRpc]
    public void OnDrop()
    {
        if (explosionRoutine != null)
        {
            StopCoroutine(explosionRoutine);
            explosionRoutine = null;
        }
        if (pickUpRoutine != null)
        {
            StopCoroutine(pickUpRoutine);
            pickUpRoutine = null;
        }

        gameObject.DrawRectangle(lineRenderer, pickUpZoneSize);
        isGettingPickedUp = false;
        isPickedUp = false;
        lineRenderer.enabled = true;
    }
    /// <summary>
    /// Command for what happens if you enter the drop zone with the bomb
    /// </summary>
    /// <param name="timeForExplosion"></param>
    /// <param name="teamWhichDroppedBomb"></param>
    [Command(requiresAuthority = false)]
    public void OnEnteringDropZone(float timeForExplosion,Team teamWhichDroppedBomb)
    {
        explosionRoutine = StartCoroutine(ExplodeBomb(timeForExplosion,teamWhichDroppedBomb));
    }
    /// <summary>
    /// Command that handles what happens when a bomb carrier leaves the bomb zone
    /// </summary>
    [Command(requiresAuthority = false)]
    public void OnPlayerExitDropZone()
    {
        if (explosionRoutine != null)
        {
            StopCoroutine(explosionRoutine);
            explosionRoutine = null;
        }
    }
    /// <summary>
    /// Starts if somebody tries pickung up the bomb
    /// </summary>
    /// <param name="carrier"></param>
    /// <returns></returns>
    private IEnumerator PickUpTimer(BombCarrier carrier)
    {
        pickupTime = this.timeForPickup;
        timer.gameObject.SetActive(isGettingPickedUp);

        while (pickupTime > 0 && Physics.CheckBox(transform.position, pickUpZoneSize / 2,transform.rotation,playerLayer) )
        {
            timer.fillAmount = pickupTime / timeForPickup;
            pickupTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if(pickupTime <= 0)
        {
            isGettingPickedUp = false;
            timer.gameObject.SetActive(isGettingPickedUp);
            carrier.carriedBomb = this;
            isPickedUp = true;
            lineRenderer.enabled = false;
        }
        else
        {
            isGettingPickedUp = false;
            timer.gameObject.SetActive(isGettingPickedUp);
            StopCoroutine(pickUpRoutine);
            pickUpRoutine = null;
        }
        pickUpRoutine = null;
    }
    /// <summary>
    /// Starts if you enter the bomb zone as a bomb carrier
    /// </summary>
    /// <param name="timeForExplosion"></param>
    /// <param name="teamWhichDroppedBomb"></param>
    /// <returns></returns>
    private IEnumerator ExplodeBomb(float timeForExplosion,Team teamWhichDroppedBomb)
    {
        Debug.Log("TimesTicking");
        yield return new WaitForSeconds(timeForExplosion);
        Debug.Log("EXploded");
        GameManager.Instance.AddPointToTeam(teamWhichDroppedBomb);
        GameManager.Instance.RoundWonCallBack();
        NetworkServer.Destroy(this.gameObject);
    }
    #region Hooks

    private void UpdateBombState(bool old,bool newValue)
    {
        isGettingPickedUp = newValue;
    }

    private void UpdateBombIsPickedUpState(bool old, bool newValue)
    {
        isPickedUp = newValue;
    }

    private void UpdateTimer(float old,float newValue)
    {
        pickupTime = newValue;
    }

    #endregion
}
