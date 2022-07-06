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
    bool startedGame = false;
    void Update()
    {
        if (startedGame) return;
        {
            CheckIfAllPlayersAreReady();
        }
       
    }
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        Debug.Log(conn.identity.GetComponent<MobaPlayer>().playerName);
    }

    void CheckIfAllPlayersAreReady()
    {

        //bool everyBodyReady = true;

        //foreach (var networkRoomPlayer in roomSlots)
        //{
        //    if (!networkRoomPlayer.readyToBegin)
        //    {
        //        everyBodyReady = false;
        //    }
        //}
        //if (everyBodyReady)
        //{
        //    startedGame = true;
        //    ServerChangeScene("EnzoScene");
        //}
    }
}
