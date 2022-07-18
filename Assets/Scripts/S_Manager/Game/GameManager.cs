using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Combat;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    [SerializeField] Transform respawnPosForBlueSide;
    [SerializeField] Transform respawnPosForRedSide;
    [SerializeField] Transform bombSpawnPos;
    [SerializeField] GameObject bombPrefab;

    readonly SyncList<MobaPlayerData> players= new SyncList<MobaPlayerData>();
    public readonly SyncDictionary<MobaPlayerData,DeathCounter> deathTimers = new SyncDictionary<MobaPlayerData, DeathCounter>();

    public event Action<MobaPlayerData> OnPlayerDie;
    public event Action<MobaPlayerData> OnPlayerRevive;
    public event Action OnRoundWon;
    public event Action OnPlayerDieUI;
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
        OnRoundWon += ResetRound;
        OnPlayerDie += StartDeathCountDown;
        OnPlayerRevive += RespawnPlayer;
    }
    void Start()
    {
        //TODO Reactivate for respawning bomb after each round
        //MobaNetworkRoomManager.SpawnPrefab(bombPrefab,bombSpawnPos);
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

    #region Callbacks
    public void RoundWonCallBack()
    {
        OnRoundWon?.Invoke();
    }
    [Command(requiresAuthority = false)]
    void ResetRound()
    {
        foreach (var player in players)
        {
            if (player.team == Team.blueSide)
            {
                player.currentlyPlayedChampion.transform.position = respawnPosForBlueSide.position;
                player.agentOfCurrentlyPlayedChampion.ResetPath();
            }
            else if (player.team == Team.redSide)
            {
                player.currentlyPlayedChampion.transform.position = respawnPosForRedSide.position;
                player.agentOfCurrentlyPlayedChampion.ResetPath();
            }
        }
        MobaNetworkRoomManager.SpawnPrefab(bombPrefab, bombSpawnPos);
    }
    public void PlayerDiedCallback(MobaPlayerData player)
    {
        OnPlayerDieUI?.Invoke();
        OnPlayerDie?.Invoke(player);
    }

    void StartDeathCountDown(MobaPlayerData player)
    {
        if (deathTimers.ContainsKey(player))
        {
            var deathTimer = deathTimers[player];

            StartCoroutine(deathTimer.StartDeathCountdown(2f, player));
        }
    }
    public void RevivePlayer(MobaPlayerData playerToRevive)
    {
        OnPlayerRevive?.Invoke(playerToRevive);
    }

    void RespawnPlayer(MobaPlayerData playerToRespawn)
    {
        playerToRespawn.currentlyPlayedChampion.GetComponent<NavMeshAgent>().ResetPath();

        if (playerToRespawn.team == Team.blueSide)
        {
            playerToRespawn.currentlyPlayedChampion.transform.position = respawnPosForBlueSide.position;
            playerToRespawn.agentOfCurrentlyPlayedChampion.ResetPath();
        }
        else if (playerToRespawn.team == Team.redSide)
        {
            playerToRespawn.currentlyPlayedChampion.transform.position = respawnPosForBlueSide.position;
            playerToRespawn.agentOfCurrentlyPlayedChampion.ResetPath();
        }
    }
    #endregion
}
