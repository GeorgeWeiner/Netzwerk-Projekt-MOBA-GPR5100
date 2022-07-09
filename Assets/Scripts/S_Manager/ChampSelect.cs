using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class ChampSelect : NetworkBehaviour
{
    [SerializeField] Transform[] championDisplaySpawnPositions;
    [SerializeField] GameObject testObject;
    public readonly SyncList<NameDisplayField> names = new SyncList<NameDisplayField>();
    public readonly SyncList<MobaPlayer> players = new SyncList<MobaPlayer>();
    public readonly SyncDictionary<MobaPlayer, Transform> championDisplayPositions = new SyncDictionary<MobaPlayer, Transform>();
    public readonly SyncDictionary<MobaPlayer,GameObject> championChoosenDisplay = new SyncDictionary<MobaPlayer, GameObject>();
    [SyncVar(hook = nameof(UpdatePlayerCount))] int playerCount;

    void Awake()
    {
        MobaNetworkRoomManager.OnPlayerEnterChampSelect += AddPlayerToChampSelect;
    }

    [Server]
    void Update()
    {
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
        championDisplayPositions.Add(playerToAdd,championDisplaySpawnPositions[playerCount]);
        playerCount++;
    }
    void UpdatePlayerCount(int old,int newPlayerCount)
    {
        playerCount = newPlayerCount;
    }
    public void ChangeChampionPrefabOfPlayer(GameObject prefabToChangeTo)
    {
        var mobaPlayer = NetworkClient.connection.identity.GetComponent<MobaPlayer>();
        mobaPlayer.ChangeChampionPrefabToSpawn(prefabToChangeTo);
    }
}
