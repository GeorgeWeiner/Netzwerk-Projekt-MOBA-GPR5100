using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : NetworkBehaviour
{
    [SerializeField] Vector3 pickUpZoneSize;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float timeForPickup;

    [SyncVar(hook = nameof(UpdateBombState))] public bool isGettingPickedUp;
    [SyncVar(hook = nameof(UpdateBombIsPickedUpState))] public bool isPickedUp = false;
    public bool IsPickedUp{ get => isPickedUp; }
    Coroutine routine;

    void Update()
    {
        Debug.Log(Physics.CheckBox(transform.position, pickUpZoneSize / 2, transform.rotation, playerLayer));
    }
    public void OnPickUp(BombCarrier carrier)
    {
        if (routine != null) return;
        {
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
        float pickupTime = this.timeForPickup;

        while (pickupTime > 0 && Physics.CheckBox(transform.position, pickUpZoneSize / 2,transform.rotation,playerLayer) )
        {
            Debug.Log("HEHEHE");
            pickupTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if(pickupTime <= 0)
        {
            carrier.carriedBomb = this;
            isPickedUp = true;
        }
        else
        {
            Debug.Log("HEHEHEStopped");
            isGettingPickedUp = false;
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

    #endregion
}
