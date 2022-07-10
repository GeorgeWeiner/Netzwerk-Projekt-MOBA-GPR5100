using Mirror;
using S_Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class ChampSelect : NetworkBehaviour
{
    [SerializeField] private Transform[] championDisplaySpawnPositions;
    public readonly SyncList<NameDisplayField> names = new();
    public readonly SyncList<MobaPlayerData> playersData = new SyncList<MobaPlayerData>();
    public readonly SyncDictionary<MobaPlayerData, Transform> championDisplayPositions = new();
    public readonly SyncDictionary<MobaPlayer,GameObject> championChoosenDisplay = new();
    [SyncVar(hook = nameof(UpdatePlayerCount))]
    private int _playerCount;

    private void Awake()
    {
        MobaNetworkRoomManager.OnPlayerEnterChampSelect += AddPlayerToChampSelect;
    }

    [Server]
    private void Update()
    {
        for (int i = 0; i < playersData.Count; i++)
        {
            if (names[i] != null)
            {
                names[i].playerName = playersData[i].playerName;
            }
        }
    }
    public void AddPlayerToChampSelect(MobaPlayerData playerToAdd)
    {
        playersData.Add(playerToAdd);
        championDisplayPositions.Add(playerToAdd, championDisplaySpawnPositions[_playerCount]);
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
               mobaPlayerData.CmdChangePrefab(championId);
            }
        }
    }
}
