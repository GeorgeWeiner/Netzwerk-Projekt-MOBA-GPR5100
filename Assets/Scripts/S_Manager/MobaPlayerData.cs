using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MobaPlayerData : NetworkBehaviour
{
    
    [SerializeField] public List<ChampionData> allChampionsAvailable;
    public readonly SyncList<int> allChampions = new SyncList<int>();

    [SyncVar(hook = nameof(ChangeCurrentChampionVisualDisplay))] GameObject visualInstance;
    [SyncVar(hook = nameof(ChangeReadyState))] bool isReady;
    public bool IsReady{ get => isReady; }
    [SyncVar(hook = nameof(ChangeName))] public string playerName;
    [SyncVar(hook = nameof(ChangeCurrentChampion))] int currentChampion;
  
    public int CurrentChampion { get => currentChampion; }


    void Awake()
    {
        AddAllChampionsAvailable();
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

    #endregion

    void AddAllChampionsAvailable()
    {
        for (int i = 0; i < allChampionsAvailable.Count; i++)
        {
            allChampions.Add(allChampionsAvailable[i].ChampionId);
        }
    }
}
