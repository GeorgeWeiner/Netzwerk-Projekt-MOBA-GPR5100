using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class Bomb : NetworkBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Image timer;
    [SerializeField] Vector3 pickUpZoneSize;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float timeForPickup;

    [SyncVar(hook = nameof(UpdateTimer))] float pickupTime;
    [SyncVar(hook = nameof(UpdateBombState))]  bool isGettingPickedUp;
    public bool IsGettingPickedUp{ get => isGettingPickedUp; }
    [SyncVar(hook = nameof(UpdateBombIsPickedUpState))]  bool isPickedUp = false;
    public bool IsPickedUp{ get => isPickedUp; }
    Coroutine pickUpRoutine;
    Coroutine explosionRoutine;

    void Start()
    {
        gameObject.DrawRectangle(lineRenderer, pickUpZoneSize);
    }
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
    [Command(requiresAuthority = false)]
    public void OnEnteringDropZone(float timeForExplosion)
    {
        Debug.Log("HEHEHEHEHDROPPED");
        explosionRoutine = StartCoroutine(ExplodeBomb(timeForExplosion));
    }
    [Command(requiresAuthority = false)]
    public void OnPlayerExitDropZone()
    {
        Debug.Log("HEHEHEHEHDROPPED2");
        if (explosionRoutine != null)
        {
            Debug.Log("HEHEHEHEHDROPPED3");
            StopCoroutine(explosionRoutine);
            explosionRoutine = null;
        }
    }
    IEnumerator PickUpTimer(BombCarrier carrier)
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

    IEnumerator ExplodeBomb(float timeForExplosion)
    {
        Debug.Log("TimesTicking");
        yield return new WaitForSeconds(timeForExplosion);
        Debug.Log("EXploded");
        GameManager.Instance.RoundWonCallBack();
        NetworkServer.Destroy(this.gameObject);
    }
    #region Hooks

    void UpdateBombState(bool old,bool newValue)
    {
        isGettingPickedUp = newValue;
    }
    void UpdateBombIsPickedUpState(bool old, bool newValue)
    {
        isPickedUp = newValue;
    }

    void UpdateTimer(float old,float newValue)
    {
        pickupTime = newValue;
    }

    #endregion
}
