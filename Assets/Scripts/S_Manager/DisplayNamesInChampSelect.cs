using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Player;
using TMPro;
using UnityEngine;

public class DisplayNamesInChampSelect : NetworkBehaviour
{
    public readonly SyncList<NameDisplayField> names = new SyncList<NameDisplayField>();
    public readonly SyncList<MobaPlayer> players = new SyncList<MobaPlayer>();

    void Awake()
    {
        MobaNetworkRoomManager.OnPlayerEnterChampSelect += AddPlayerToChampSelect;
    }

    [Server]
    void Update()
    {
        Debug.Log(names.Count);
        for (int i = 0; i < players.Count; i++)
        {
            if (names[i] != null)
            {
                names[i].playerName = players[i].playerName;
            }

        }
    }

    public void AddPlayerToChampSelect(MobaPlayer playerToAdd)
    {
        players.Add(playerToAdd);
    }
}
