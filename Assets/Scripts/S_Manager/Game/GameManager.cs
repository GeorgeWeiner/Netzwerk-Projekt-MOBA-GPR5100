using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{ 
    public readonly SyncList<MobaPlayerData> objects = new SyncList<MobaPlayerData>();
    static public event Action<MobaPlayerData> OnClientEnterGame;
    static public GameManager instance;

    void Awake()
    {
        
    }

    public override void OnStartServer()
    {

    }
}
