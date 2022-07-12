using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    readonly SyncList<MobaPlayerData> players= new SyncList<MobaPlayerData>();
    public readonly SyncDictionary<MobaPlayerData,DeathCounter> deathTimers = new SyncDictionary<MobaPlayerData, DeathCounter>();
    static public event Action<MobaPlayerData> OnPlayerDie;
    static public event Action OnPlayerDieUI;
    static public GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        
        OnPlayerDie += StartDeathCountDown;
    }
    void OnDestroy()
    {
        OnPlayerDie -= StartDeathCountDown;
    }
    #region EventCallbacks

    public void AddPlayerToAllPlayersList(MobaPlayerData data)
    {
        players.Add(data);
        deathTimers.Add(data, data.gameObject.GetComponent<DeathCounter>());
    }
   
    #endregion
    #region PlayerCallbacks
    static public void PlayerDiedCallback(MobaPlayerData player)
    {
        OnPlayerDieUI?.Invoke();
        OnPlayerDie?.Invoke(player);
    }

    void StartDeathCountDown(MobaPlayerData player)
    {
        if (deathTimers.ContainsKey(player))
        {
            var deathTimer = deathTimers[player];

            Debug.Log("started");
            StartCoroutine(deathTimer.StartDeathCountdown(2f, player));
        }
    }
    void RevivePlayer(GameObject player)
    {

    }

    #endregion
}
