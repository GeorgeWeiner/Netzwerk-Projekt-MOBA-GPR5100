using System;
using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// handles the whole champ select process
/// </summary>
public class ChampSelect : BaseNetworkBehaviourSingleton<ChampSelect>
{
    [SerializeField] private SubmitName nameSubmitButton;
    [SerializeField] private Button startButton;
    [SerializeField] private NameDisplayField[] nameDisplayFields;
    [SerializeField] private Transform[] championDisplaySpawnPositions;
    [SerializeField][Scene] private string sceneToChangeTo;
    public readonly SyncDictionary<MobaPlayerData, NameDisplayField> names = new();
    public readonly SyncList<MobaPlayerData> playersData = new SyncList<MobaPlayerData>();
    public readonly SyncDictionary<MobaPlayerData, Transform> championDisplayPositions = new();
    private Coroutine routine;
    [Server]
    private void Awake()
    {
        MobaNetworkRoomManager.OnPlayerEnterChampSelect += AddPlayerToChampSelect;
    }

    [Server]
    private void Start()
    {
        SetHostMenuInActive(startButton.gameObject);
        StartCoroutine(OnPlayerLeaveGame());
    }

    private void Update()
    {
        UpdateDisplayedNames();
    }

    private void OnDestroy()
    {
        MobaNetworkRoomManager.OnPlayerEnterChampSelect -= AddPlayerToChampSelect;
    }
    /// <summary>
    /// Adds the players to the collections so that you can use and manipulate their data
    /// </summary>
    /// <param name="playerToAdd"></param>
    public void AddPlayerToChampSelect(MobaPlayerData playerToAdd)
    {
        if (!playersData.Contains(playerToAdd) && !names.ContainsKey(playerToAdd))
        {
            playerToAdd.playerNumber = NetworkManager.singleton.numPlayers;
            names.Add(playerToAdd, nameDisplayFields[NetworkManager.singleton.numPlayers - 1]);
            championDisplayPositions.Add(playerToAdd, championDisplaySpawnPositions[NetworkManager.singleton.numPlayers - 1]);
            playersData.Add(playerToAdd);
        }
        if (NetworkManager.singleton.numPlayers <= 1)
        {
            playerToAdd.team = Team.blueSide;
        }
        else
        {
            playerToAdd.team = Team.redSide;
        }
    }
    /// <summary>
    /// Changes the champion prefab of a player to the champion with the cahmpion id
    /// </summary>
    /// <param name="championId"></param>
    public void ChangeChampionPrefabOfPlayer(int championId)
    {
        foreach (var mobaPlayerData in playersData)
        {
            if (mobaPlayerData == NetworkClient.connection.identity.GetComponent<MobaPlayerData>())
            {
                mobaPlayerData.CmdChangePrefab(championId, championDisplayPositions[mobaPlayerData].position);
                mobaPlayerData.CmdChangeReadyState(true);
            }
        }
    }
    /// <summary>
    /// Sets the host menu for starting the game inactive if youre a client
    /// </summary>
    /// <param name="menu"></param>
    public void SetHostMenuInActive(GameObject menu)
    {
        if (NetworkClient.connection.identity.isClientOnly)
        {
            menu.SetActive(false);
        }
        else
        {
            menu.SetActive(true);
        }
    }
    /// <summary>
    /// Changes the name of a given player
    /// </summary>
    /// <param name="nameToChangeTo"></param>
    public void UpdatePlayersName(string nameToChangeTo)
    {
        foreach (var mobaPlayerData in playersData)
        {
            if (mobaPlayerData == NetworkClient.connection.identity.GetComponent<MobaPlayerData>())
            {
                mobaPlayerData.CmdChangeName(nameToChangeTo);
            }
        }
    }
    /// <summary>
    /// Updates the displaed names in champ select visually by looping over each player
    /// </summary>
    private void UpdateDisplayedNames()
    {
        for (int i = 0; i < playersData.Count; i++)
        {
            if (names.ContainsKey(playersData[i]))
            {
                names[playersData[i]].playerName = playersData[i].playerName;
            }
        }
    }
    /// <summary>
    /// Starts the game if there are enough players in the lobby and everybody is ready means selected a champion
    /// </summary>
    public void StartGame()
    {
        bool allPlayersReady = true;

        foreach (var player in playersData)
        {
            if (!player.IsReady)
            {
                allPlayersReady = false;
                return;
            }
        }

        if (allPlayersReady /*&& NetworkManager.singleton.numPlayers == 2*/)
        {
            NetworkManager.singleton.ServerChangeScene(sceneToChangeTo);
        }

    }
    /// <summary>
    /// Leaves the game you cann reconnect to the champ select
    /// </summary>
    public void LeaveGame()
    {

        if (!NetworkClient.isHostClient)
        {
            NetworkClient.DestroyAllClientObjects();
            NetworkClient.Disconnect();
            SceneManager.LoadScene("LobbyScene");
        }
        else
        {
            NetworkClient.DestroyAllClientObjects();
            MobaNetworkRoomManager.singleton.StopHost();
            SceneManager.LoadScene("LobbyScene");
        }
    }
    /// <summary>
    /// Coroutine that starts at the beginning and checks if somebody left the game and clears him from the list
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnPlayerLeaveGame()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < playersData.Count; i++)
        {
            if (playersData[i] == null)
            {
                names[playersData[i]].playerName = String.Empty;
                championDisplayPositions.Remove(playersData[i]);
                names.Remove(playersData[i]);
                playersData.Remove(playersData[i]);
            }
        }
        StartCoroutine(OnPlayerLeaveGame());
    }
}
