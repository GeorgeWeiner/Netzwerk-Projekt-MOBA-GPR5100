using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Team
{
    redSide,
    blueSide
}
public class MobaPlayerData : NetworkBehaviour
{
    
    [SerializeField] public List<ChampionData> allChampionsAvailable;
    public readonly SyncList<int> allChampions = new SyncList<int>();

    [HideInInspector] [SyncVar(hook = nameof(ChangeCurrentChampionVisualDisplay))] GameObject visualInstance;
    [HideInInspector] [SyncVar(hook = nameof(ChangeCurrentlyPlayedChampion) )] public GameObject currentlyPlayedChampion;
    [HideInInspector] [SyncVar(hook = nameof(ChangeCurrentChampion))] int currentChampion;
    public int CurrentChampion { get => currentChampion; }
    [HideInInspector] [SyncVar(hook = nameof(ChangeReadyState))] bool isReady;
    public bool IsReady { get => isReady; }

    [SyncVar(hook = nameof(ChangeName))] public string playerName;
    [SyncVar(hook = nameof(ChangeTeam))] public Team team;
    static public event Action<MobaPlayerData> OnPlayerEnterGame;

    void Awake()
    {
        AddAllChampionsAvailable();
    }
    public void ChangedSceneToGameScene()
    {
        OnPlayerEnterGame?.Invoke(this);
    }

    #region Commands
    [Command]
    public void CmdChangePrefab(int championToSpawn,Vector3 position)
    {
        if (allChampions.Contains(allChampionsAvailable[championToSpawn].ChampionId))
        {
            if (visualInstance != null)
            {
                NetworkServer.Destroy(visualInstance);
            }
            var instance = Instantiate(allChampionsAvailable[championToSpawn].GetVisualDisplay(),position,Quaternion.identity);
            NetworkServer.Spawn(instance);
            currentChampion = allChampionsAvailable[championToSpawn].ChampionId;
            visualInstance = instance;
        }
    }
    [Command]
    public void DestroyChampionOnLeave(MobaPlayerData playerData)
    {
        NetworkServer.Destroy(playerData.visualInstance);
    }
    [Command]
    public void CmdChangeName(string newName)
    {
        playerName = newName;
    }

    [Command]
    public void CmdChangeReadyState(bool isReady)
    {
        this.isReady = isReady;
    }

    #endregion

    #region Hooks

    public void ChangeCurrentChampionVisualDisplay(GameObject old,GameObject newObject)
    {
        visualInstance = newObject;
    }
    public void ChangeCurrentlyPlayedChampion(GameObject old, GameObject newObject)
    {
        currentlyPlayedChampion = newObject;
    }
    public void ChangeCurrentChampion(int old , int newChampionId)
    {
        currentChampion = newChampionId;
    }
    public void ChangeName(string old, string newName)
    {
        playerName = newName;
    }

    public void ChangeReadyState(bool old,bool newState)
    {
        isReady = newState;
    }
    public void ChangeTeam(Team old,Team newTeam)
    {
        this.team = newTeam;
    }

    #endregion

    #region Client

    void AddAllChampionsAvailable()
    {
        for (int i = 0; i < allChampionsAvailable.Count; i++)
        {
            allChampions.Add(allChampionsAvailable[i].ChampionId);
        }
    }

    #endregion

}
