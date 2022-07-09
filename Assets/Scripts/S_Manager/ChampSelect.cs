using Mirror;
using S_Player;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class ChampSelect : NetworkBehaviour
{
    [SerializeField] private Transform[] championDisplaySpawnPositions;
    [SerializeField] private GameObject testObject;
    public readonly SyncList<NameDisplayField> names = new();
    public readonly SyncList<MobaPlayer> players = new();
    public readonly SyncDictionary<MobaPlayer, Transform> championDisplayPositions = new();
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
        for (int i = 0; i < players.Count; i++)
        {
            if (names[i] != null)
            {
                names[i].playerName = players[i].playerName;
            }
        }
    }
    public void AddPlayerToChampSelect(MobaPlayer playerToAdd)
    {
        players.Add(playerToAdd);
        championDisplayPositions.Add(playerToAdd,championDisplaySpawnPositions[_playerCount]);
        _playerCount++;
    }

    private void UpdatePlayerCount(int old,int newPlayerCount)
    {
        _playerCount = newPlayerCount;
    }
    
    public void ChangeChampionPrefabOfPlayer(GameObject prefabToChangeTo)
    {
        var mobaPlayer = NetworkClient.connection.identity.GetComponent<MobaPlayer>();
        mobaPlayer.ChangeChampionPrefabToSpawn(prefabToChangeTo);
    }
}
