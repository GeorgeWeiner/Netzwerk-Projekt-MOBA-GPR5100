using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public enum StatType
{
    health,
    mana,
    attackValue,
    defenseValue,
}
public abstract class Stat : NetworkBehaviour, ICharacterStat
{
    [SerializeField] StatType statType;
    public StatType StatType{ get => statType; }
    [SyncVar][SerializeField] int maxValue;
    public int MaxValue{ get => maxValue; }
    [SyncVar(hook = nameof(HandleStatUpdated))] [SerializeField] protected int currentValue;
    public int CurrentValue{ get => currentValue; }
    public event Action<StatType, int,int> ClientOnStatUpdated;
    protected MobaPlayerData playerDataForCallbacks;

    void Start()
    {
        playerDataForCallbacks = NetworkClient.connection.identity.GetComponent<MobaPlayerData>();
        Debug.Log(playerDataForCallbacks.currentlyPlayedChampion);
        currentValue = maxValue;
        Debug.Log(currentValue);
    }
    public virtual void HandleStatUpdated(int oldValue, int newValue)
    {
        currentValue = newValue;
        ClientOnStatUpdated?.Invoke(statType,currentValue,maxValue);
    }

    public void HandleMaxValueUpdated(int old,int newMaxValue)
    {
        maxValue = newMaxValue;
    }
}
