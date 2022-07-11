using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{ 
    readonly SyncList<MobaPlayerData> players= new SyncList<MobaPlayerData>();
    readonly SyncDictionary<MobaPlayerData,DeathCounter> deathTimers = new SyncDictionary<MobaPlayerData, DeathCounter>();
    static public event Action<MobaPlayerData> OnPlayerDie;
    static public GameManager instance;

    void Awake()
    {
        MobaPlayerData.OnPlayerEnterGame += AddPlayerToAllPlayersList;
        OnPlayerDie += StartDeathCountDown;
    }
    void Start()
    {
        var player = NetworkClient.connection.identity.GetComponent<MobaPlayerData>();
       
    }
    static public void PlayerDiedCallback(MobaPlayerData player)
    {
        OnPlayerDie?.Invoke(player);
    }

    void AddPlayerToAllPlayersList(MobaPlayerData data)
    {
        players.Add(data);
        deathTimers.Add(data,data.gameObject.GetComponent<DeathCounter>());
    }

    void OnDestroy()
    {
        MobaPlayerData.OnPlayerEnterGame -= AddPlayerToAllPlayersList;
        OnPlayerDie -= StartDeathCountDown;
    }

    void StartDeathCountDown(MobaPlayerData player)
    {
        if (deathTimers.ContainsKey(player))
        {
            var deathTimer = deathTimers[player];
           
            Debug.Log("started");
            StartCoroutine(deathTimer.StartDeathCountdown(2f,player));
        }
    }

    void RevivePlayer(GameObject player)
    {

    }
}
