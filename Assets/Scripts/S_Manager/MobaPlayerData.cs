using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MobaPlayerData : NetworkBehaviour
{
    [SyncVar(hook = nameof(ChangeChampionPrefab))] public GameObject championPrefab;
    [SyncVar(hook = nameof(ChangeName))] public string playerName;
    [SerializeField] List<GameObject> allChampionsAvailable;
    public readonly SyncList<int> allChampions = new SyncList<int>();
    void Awake()
    {
        AddAllChampionsAvailable();
    }
    #region Commands


    [Command]
    public void CmdChangePrefab(int championToSpawn)
    {
        if (allChampions.Contains(championToSpawn))
        {
            var instance = Instantiate(allChampionsAvailable[championToSpawn]);
            NetworkServer.Spawn(instance);
            championPrefab = instance;
        }
    }
    [Command]
    public void CmdChangeName(string newName)
    {
        playerName = newName;
    }

    #endregion

    #region Hooks

    public void ChangeChampionPrefab(GameObject old, GameObject newObject)
    {
        championPrefab = newObject;
    }

    public void ChangeName(string old, string newName)
    {
        playerName = newName;
    }

    #endregion

    void AddAllChampionsAvailable()
    {
        for (int i = 0; i < allChampionsAvailable.Count; i++)
        {
            allChampions.Add(i);
        }
    }
}
