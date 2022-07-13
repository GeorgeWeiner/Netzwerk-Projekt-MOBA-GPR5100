using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Player;
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
    public readonly SyncDictionary<MobaPlayerData, NameDisplayField> names = new();
    public readonly SyncList<MobaPlayerData> playersData = new SyncList<MobaPlayerData>();
    public readonly SyncDictionary<MobaPlayerData, Transform> championDisplayPositions = new();

    [Server]
    private void Awake()
    {
        MobaNetworkRoomManager.OnPlayerEnterChampSelect += AddPlayerToChampSelect;
        MobaNetworkRoomManager.OnPlayerDisconnect += LeaveGame;
    }

    [Server]
    void Start()
    {
        SetHostMenuInActive(startButton.gameObject);
    }

    void Update()
    {
        UpdateDisplayedNames();
    }

    void OnDestroy()
    {
        MobaNetworkRoomManager.OnPlayerEnterChampSelect -= AddPlayerToChampSelect;
        MobaNetworkRoomManager.OnPlayerDisconnect -= LeaveGame;
    }

    public void AddPlayerToChampSelect(MobaPlayerData playerToAdd)
    {
        playersData.Add(playerToAdd);
        championDisplayPositions.Add(playerToAdd,
            championDisplaySpawnPositions[NetworkManager.singleton.numPlayers - 1]);
        names.Add(playerToAdd, nameDisplayFields[NetworkManager.singleton.numPlayers - 1]);

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
            names[playersData[i]].playerName = playersData[i].playerName;
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

        if (allPlayersReady && MobaNetworkRoomManager.singleton.numPlayers == 2)
        {
            MobaNetworkRoomManager.singleton.ServerChangeScene("EnzoScene");
        }

    }

    public void LeaveButton()
    {
        var networkManager = FindObjectOfType<MobaNetworkRoomManager>();
        networkManager.CallBackForDisconnect();
    }

    void LeaveGame(MobaPlayerData player)
    {
        Debug.Log("REACHED");
        playersData.Remove(player);
        championDisplayPositions.Remove(player);
        names.Remove(player);
        StartCoroutine(Disconnect());
    }

    IEnumerator Disconnect()
    {
        yield return new WaitForSeconds(0.2f);

        if (!NetworkClient.isHostClient)
        {
            NetworkClient.DestroyAllClientObjects();
            NetworkClient.Disconnect();
            //SceneManager.LoadScene("LobbyScene");


        }
        else
        {
            //NetworkClient.DestroyAllClientObjects();
            //MobaNetworkRoomManager.singleton.StopHost();
            //SceneManager.LoadScene("LobbyScene");
        }

        yield return new WaitForSeconds(0.5f);
    }
}
