using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Player;
using Telepathy;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class ChampSelect : BaseNetworkBehaviourSingleton<ChampSelect>
{
    [SerializeField] SubmitName nameSubmitButton;
    [SerializeField] Button startButton;
    [SerializeField] NameDisplayField[] nameDisplayFields;
    [SerializeField] Transform[] championDisplaySpawnPositions;
    [SerializeField] private string sceneToChangeTo;
    public readonly SyncDictionary<MobaPlayerData, NameDisplayField> names = new();
    public readonly SyncList<MobaPlayerData> playersData = new SyncList<MobaPlayerData>();
    public readonly SyncDictionary<MobaPlayerData, Transform> championDisplayPositions = new();
    Coroutine routine;
    [Server]
    private void Awake()
    {
        MobaNetworkRoomManager.OnPlayerEnterChampSelect += AddPlayerToChampSelect;
    }

    [Server]
    void Start()
    {
        SetHostMenuInActive(startButton.gameObject);
        StartCoroutine(OnPlayerLeaveGame());
    }

    void Update()
    {
        UpdateDisplayedNames();
    }

    void OnDestroy()
    {
        MobaNetworkRoomManager.OnPlayerEnterChampSelect -= AddPlayerToChampSelect;
    }

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

    void UpdateDisplayedNames()
    {
        for (int i = 0; i < playersData.Count; i++)
        {
            if (names.ContainsKey(playersData[i]))
            {
                names[playersData[i]].playerName = playersData[i].playerName;
            }
        }
    }

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
    IEnumerator OnPlayerLeaveGame()
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
