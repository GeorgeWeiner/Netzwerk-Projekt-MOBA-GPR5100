using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Combat;
using S_Player;
using S_Unit;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class MobaNetworkRoomManager : NetworkRoomManager
{
    static public event Action<MobaPlayerData> OnPlayerEnterChampSelect;
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        MobaPlayerData playerData = conn.identity.GetComponent<MobaPlayerData>();
        OnPlayerEnterChampSelect?.Invoke(playerData);
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        return conn.identity.gameObject;
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (SceneManager.GetActiveScene().name == "EnzoScene")
        {
            foreach (var networkRoomPlayer in roomSlots)
            {
                var player = networkRoomPlayer.GetComponent<MobaPlayerData>();
                var instance = Instantiate(player.allChampionsAvailable[player.CurrentChampion].GetCurrentChampion(),startPositions[networkRoomPlayer.index].position,Quaternion.identity);
                instance.GetComponent<Targetable>().CurrentTeam = player.team;
                NetworkServer.Spawn(instance,player.connectionToClient);
                player.currentlyPlayedChampion = instance;
            }
        }
    }

    public static void SpawnPrefab(GameObject go)
    {
        var goInstance = Instantiate(go);
        NetworkServer.Spawn(goInstance);
    }
        
    public static void SpawnPrefab(GameObject go, NetworkConnectionToClient conn)
    {
        var goInstance = Instantiate(go);
        NetworkServer.Spawn(goInstance, conn);
    }
}
