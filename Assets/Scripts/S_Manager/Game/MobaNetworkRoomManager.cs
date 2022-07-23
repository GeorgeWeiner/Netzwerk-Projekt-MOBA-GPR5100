using System;
using Mirror;
using S_Combat;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MobaNetworkRoomManager : NetworkRoomManager
{
    public static event Action<MobaPlayerData> OnPlayerEnterChampSelect;

    /// <summary>
    /// sets the player data and invokes a event to which everybody who needs the player data listens like for list etc
    /// </summary>
    /// <param name="conn"></param>
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        MobaPlayerData playerData = conn.identity.GetComponent<MobaPlayerData>();
        OnPlayerEnterChampSelect?.Invoke(playerData);
    }
    /// <summary>
    /// Instead of crating a new player everytime we just keep our room player if we switch scenes
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="roomPlayer"></param>
    /// <returns></returns>
    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        return conn.identity.gameObject;
    }
    /// <summary>
    /// If its our gameplay scene we create all the players
    /// </summary>
    /// <param name="sceneName"></param>
    public override void OnServerSceneChanged(string sceneName)
    {
        if (SceneManager.GetActiveScene().path == GameplayScene)
        {
            print(GameplayScene);
            CreatePlayers();
        }
    }
    /// <summary>
    /// spawns any prefab on the server
    /// </summary>
    /// <param name="go"></param>
    public static void SpawnPrefab(GameObject go)
    {
        var goInstance = Instantiate(go);
        NetworkServer.Spawn(goInstance);
    }
    /// <summary>
    /// spawns a prefab with the given spawnPoint
    /// </summary>
    /// <param name="go"></param>
    /// <param name="spawnPoint"></param>
    public static void SpawnPrefab(GameObject go,Transform spawnPoint)
    {
        var goInstance = Instantiate(go,spawnPoint.position,Quaternion.identity);
        NetworkServer.Spawn(goInstance);
    }
    /// <summary>
    /// Spawns a  single projectile
    /// </summary>
    /// <param name="go"></param>
    /// <param name="position"></param>
    /// <param name="conn"></param>
    public static void SpawnPrefab(GameObject go,Transform position, NetworkConnectionToClient conn,int dmg,float projectileSpeed,Targetable target)
    {
        var goInstance = Instantiate(go,position.position,Quaternion.identity);
        goInstance.GetComponent<Projectile>().Initialize(position,target,dmg,projectileSpeed);
        if (conn != null)
        {
            NetworkServer.Spawn(goInstance, conn);
        }
        else
        {
            NetworkServer.Spawn(goInstance);
        }
       
    }
    /// <summary>
    /// Creates all players bzw instantiates their playable champion
    /// </summary>
    private void CreatePlayers()
    {
        //Gets the network roomPlayer and spawns in their choosen champion
        foreach (var networkRoomPlayer in roomSlots)
        {
            var player = networkRoomPlayer.GetComponent<MobaPlayerData>();
            var instance = Instantiate(player.allChampionsAvailable[player.CurrentChampion].GetCurrentChampion(), startPositions[networkRoomPlayer.index].position, Quaternion.identity);
            var stats = instance.GetComponents<Stat>();
            foreach (var stat in stats)
            {
                stat.playerDataForCallbacks = player;
            }
            instance.GetComponent<Targetable>().CurrentTeam = player.team;
            NetworkServer.Spawn(instance, player.connectionToClient);
            player.currentlyPlayedChampion = instance;
            //player.currentlyPlayedChampion.GetComponent<NetworkIdentity>()
            //    .AssignClientAuthority(networkRoomPlayer.connectionToClient);
            GameManager.Instance.AddPlayerToAllPlayersList(player);
        }
    }
}
