using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Player;
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

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        if (SceneManager.GetActiveScene().name == "EnzoScene")
        {
            foreach (var networkRoomPlayer in roomSlots)
            {
                var player = networkRoomPlayer.GetComponent<MobaPlayerData>();
                var instance = Instantiate(player.allChampionsAvailable[player.CurrentChampion].GetCurrentChampion());
                NetworkServer.Spawn(instance,player.connectionToClient);


            }
        }
       
    }
}
