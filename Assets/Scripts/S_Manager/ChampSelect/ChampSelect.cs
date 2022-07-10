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

public class ChampSelect : NetworkBehaviour
{
    [SerializeField] SubmitName nameSubmitButton;
    [SerializeField] Button startButton;
    [SerializeField] NameDisplayField[] nameDisplayFields;
    [SerializeField] Transform[] championDisplaySpawnPositions;
    public readonly SyncDictionary<MobaPlayerData,NameDisplayField> names = new();
    public readonly SyncList<MobaPlayerData> playersData = new SyncList<MobaPlayerData>();
    public readonly SyncDictionary<MobaPlayerData, Transform> championDisplayPositions = new();
    public readonly SyncDictionary<MobaPlayer,GameObject> championChoosenDisplay = new();
    [SyncVar(hook = nameof(UpdatePlayerCount))] public int _playerCount = 0;
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
    }

    [Server]
    void Update()
    {
        UpdateDisplayedName();
    }
    public void AddPlayerToChampSelect(MobaPlayerData playerToAdd)
    {
       
        playersData.Add(playerToAdd);
        championDisplayPositions.Add(playerToAdd, championDisplaySpawnPositions[_playerCount]);
        names.Add(playerToAdd,nameDisplayFields[_playerCount]);
        if (_playerCount < 1)
        {
            playerToAdd.team = Team.blueSide;
        }
        else
        {
            playerToAdd.team = Team.redSide;
        }
        _playerCount++;

    }
    private void UpdatePlayerCount(int old,int newPlayerCount)
    {
        _playerCount = newPlayerCount;
    }
    public void ChangeChampionPrefabOfPlayer(int championId)
    {
        foreach (var mobaPlayerData in playersData)
        {
            if (mobaPlayerData == NetworkClient.connection.identity.GetComponent<MobaPlayerData>())
            {
               mobaPlayerData.CmdChangePrefab(championId,championDisplayPositions[mobaPlayerData].position);
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
        if (allPlayersReady && _playerCount == 2)
        {
            MobaNetworkRoomManager.singleton.ServerChangeScene("EnzoScene");
        }
       
    } 
    public void UpdateDisplayedName()
    {
        foreach (var nameDisplayField in names)
        {
            nameDisplayField.Value.playerName = nameDisplayField.Key.playerName;
        }
    }
    public void LeaveGame()
    {
        if (!NetworkClient.isHostClient && routine == null)
        {
            var playerToRemove = NetworkClient.connection.identity.GetComponent<MobaPlayerData>();
            playersData.Remove(playerToRemove);
            championDisplayPositions.Remove(playerToRemove);
            names.Remove(playerToRemove);
            _playerCount--;
            routine = StartCoroutine(LoadScene());
            Debug.Log("HEHEHEHEEHHEHEH");
        }
        else if(routine == null)
        {
            var playerToRemove = NetworkClient.connection.identity.GetComponent<MobaPlayerData>();
            playersData.Remove(playerToRemove);
            championDisplayPositions.Remove(playerToRemove);
            names.Remove(playerToRemove);
            _playerCount--;
            routine = StartCoroutine(LoadScene());
        }
      StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1f);
        if (!NetworkClient.isHostClient)
        {
            NetworkClient.Disconnect();
            SceneManager.LoadScene("LobbyScene");
        }
        else
        {
            MobaNetworkRoomManager.singleton.StopHost();
            SceneManager.LoadScene("LobbyScene");
        }
    }
}
