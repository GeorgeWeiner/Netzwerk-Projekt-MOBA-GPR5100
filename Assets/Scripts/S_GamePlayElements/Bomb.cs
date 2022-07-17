using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class Bomb : NetworkBehaviour
{
    [SerializeField] Image timer;
    [SerializeField] Vector3 pickUpZoneSize;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float timeForPickup;

    [SyncVar(hook = nameof(UpdateTimer))] float pickupTime;
    [SyncVar(hook = nameof(UpdateBombState))] public bool isGettingPickedUp;
    [SyncVar(hook = nameof(UpdateBombIsPickedUpState))] public bool isPickedUp = false;
    public bool IsPickedUp{ get => isPickedUp; }
    Coroutine routine;

    [ClientRpc]
    public void OnPickUp(BombCarrier carrier)
    {
        if (routine != null) return;
        {
            isGettingPickedUp = true;
            routine = StartCoroutine(PickUpTimer(carrier));
        }
    }
    public void OnDrop()
    {
        isGettingPickedUp = false;
        isPickedUp = false;
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
        }
        else
        {
            isGettingPickedUp = false;
            timer.gameObject.SetActive(isGettingPickedUp);
            StopCoroutine(routine);
            routine = null;
        }
        routine = null;
    }

    void OnDrawGizmos()
    {
        if (!isPickedUp)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(transform.position, pickUpZoneSize);
        }
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
