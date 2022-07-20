using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Combat;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
/// <summary>
/// Handles the whole game flow from reviving players to counting points etc
/// </summary>
public class GameManager : NetworkBehaviour
{
    [SerializeField] TMP_Text displayForWhoWonGame;
    [SerializeField] Transform respawnPosForBlueSide;
    [SerializeField] Transform respawnPosForRedSide;
    [SerializeField] Transform bombSpawnPos;//the span pos for the bomb when the round gets reseted
    [SerializeField] GameObject bombPrefab;

    readonly SyncList<MobaPlayerData> players= new SyncList<MobaPlayerData>();// stores all players data for reseting them or getting their team etc
    public readonly SyncDictionary<Team, int> points = new();//stores both teams and their points
    public readonly SyncDictionary<MobaPlayerData,DeathCounter> deathTimers = new SyncDictionary<MobaPlayerData, DeathCounter>();// stores all the deathtimers with the specific player
    
    public event Action<MobaPlayerData> OnPlayerDie;// for disabling the players ability to move etc
    public event Action<MobaPlayerData> OnPlayerRevive;//Same but while the round is active for resetting stats usw
    public event Action OnRoundWon;// event to which you cann hook up when you want to reset smthing like health etc when smbody won the round
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
        OnRoundWon += DisplayWhoWonGame;
        OnPlayerDie += StartDeathCountDown;
        OnPlayerRevive += RespawnPlayer;
    }
    /// <summary>
    /// Initializes the points sync dictionairy
    /// </summary>
    [Server]
    void Start()
    {
        if (!points.ContainsKey(Team.blueSide) || points.ContainsKey(Team.redSide))
        {
            points.Add(Team.blueSide, 0);
            points.Add(Team.redSide, 0);
        }
        //TODO Reactivate for respawning bomb after each round
        //MobaNetworkRoomManager.SpawnPrefab(bombPrefab,bombSpawnPos);
    }
    void OnDestroy()
    {
        OnRoundWon -= ResetRound;
        OnRoundWon -= DisplayWhoWonGame;
        OnPlayerDie -= StartDeathCountDown;
        OnPlayerRevive -= RespawnPlayer;
    }
    #region EventCallbacks

    public void AddPlayerToAllPlayersList(MobaPlayerData data)
    {
        players.Add(data);
        deathTimers.Add(data, data.gameObject.GetComponent<DeathCounter>());
    }

    #endregion

    #region Callbacks
    /// <summary>
    /// Callback which gets called from the bomb when a player explodes the bomb
    /// </summary>
    public void RoundWonCallBack()
    {
        OnRoundWon?.Invoke();
    }
    /// <summary>
    /// Resets the round by reseting the players positions 
    /// </summary>
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
    /// <summary>
    /// Call bcak when a player dies which ionvokes events for the ui and the player
    /// </summary>
    /// <param name="player"></param>
    public void PlayerDiedCallback(MobaPlayerData player)
    {
        OnPlayerDieUI?.Invoke();
        OnPlayerDie?.Invoke(player);
    }
    /// <summary>
    /// Starts the death countdown of a specific player
    /// </summary>
    /// <param name="player"></param>
    void StartDeathCountDown(MobaPlayerData player)
    {
        if (deathTimers.ContainsKey(player))
        {
            var deathTimer = deathTimers[player];

            StartCoroutine(deathTimer.StartDeathCountdown(2f, player));
        }
    }
    /// <summary>
    /// Revive callback for when the death timer runs out etc
    /// </summary>
    /// <param name="playerToRevive"></param>
    public void RevivePlayer(MobaPlayerData playerToRevive)
    {
        OnPlayerRevive?.Invoke(playerToRevive);
    }
    /// <summary>
    /// Respawns a player on their given side and resets their way point theyve got
    /// </summary>
    /// <param name="playerToRespawn"></param>
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
    /// <summary>
    /// Adds a point to the given team
    /// </summary>
    /// <param name="team"></param>
    [Command(requiresAuthority = false)]
    public void AddPointToTeam(Team team)
    {
        if (points[team] < 3)
        {
            points[team] += 1;
        }
    }

    [ClientRpc]
    void DisplayWhoWonGame()
    {
        StartCoroutine(CheckIfSomebodyWonGame());
    }

    IEnumerator CheckIfSomebodyWonGame()
    {
        yield return new WaitForSeconds(2f);
        foreach (var team in points)
        {
            if (team.Key == Team.blueSide && team.Value == 1)
            {
                displayForWhoWonGame.gameObject.SetActive(true);
                displayForWhoWonGame.text = "Blue Team Won";
                StartCoroutine(ReloadLobbyScene());
            }
            else if (team.Key == Team.blueSide && team.Value == 3)
            {
                displayForWhoWonGame.gameObject.SetActive(true);
                displayForWhoWonGame.text = "Red Team Won";
                StartCoroutine(ReloadLobbyScene());
            }
        }
    }
    IEnumerator ReloadLobbyScene()
    {
        yield return new WaitForSeconds(5f);
        NetworkClient.Disconnect();
        if (NetworkClient.isHostClient)
        {
            NetworkManager.singleton.StartHost();
        }
        NetworkManager.singleton.ServerChangeScene("LobbyScene");

    }
    #endregion
}
