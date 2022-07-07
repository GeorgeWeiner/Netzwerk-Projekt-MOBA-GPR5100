using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Player;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class MobaNetworkRoomManager : NetworkRoomManager
{
    [SerializeField] private GameObject heroPrefab;
    static public event Action<MobaPlayer> OnPlayerEnterChampSelect;
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        OnPlayerEnterChampSelect?.Invoke(conn.identity.GetComponent<MobaPlayer>());
    }
}
