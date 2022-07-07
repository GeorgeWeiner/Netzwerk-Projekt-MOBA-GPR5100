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
    static public List<MobaPlayer> players = new List<MobaPlayer>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        players.Add(conn.identity.GetComponent<MobaPlayer>());
        Debug.Log(players.Count);
      
    }

}
