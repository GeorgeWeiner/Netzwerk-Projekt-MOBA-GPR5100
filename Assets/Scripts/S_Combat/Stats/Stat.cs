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
/// <summary>
/// Baseclass for all stats
/// </summary>
public abstract class Stat : NetworkBehaviour, ICharacterStat
{
    [SerializeField] StatType statType;
    public StatType StatType{ get => statType; }
    [SyncVar][SerializeField] int maxValue;
    public int MaxValue{ get => maxValue; }
    [SyncVar(hook = nameof(HandleStatUpdated))] [SerializeField] protected int currentValue;
    public int CurrentValue{ get => currentValue; }
    public int statRegenAmount;
    public float statRegenTick;
    public event Action<StatType, int,int> ClientOnStatUpdated;
    [SyncVar(hook = nameof(ChangePlayerDataForCallBacks))]public MobaPlayerData playerDataForCallbacks;

    void Start()
    {
        currentValue = maxValue;
        StartCoroutine(PassiveStatRegen());
    }
    /// <summary>
    /// hook for the stat Update also invokes an event for cllients to listen to like ui etc
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public virtual void HandleStatUpdated(int oldValue, int newValue)
    {
        currentValue = newValue;
        ClientOnStatUpdated?.Invoke(statType,currentValue,maxValue);
    }
    /// <summary>
    /// Hook for syncong the max health for example if we lvl up or something else
    /// </summary>
    /// <param name="old"></param>
    /// <param name="newMaxValue"></param>
    public void HandleMaxValueUpdated(int old,int newMaxValue)
    {
        maxValue = newMaxValue;
    }

    protected virtual IEnumerator PassiveStatRegen()
    {
        while (true)
        {
            currentValue += statRegenAmount;
            currentValue = Mathf.Min(maxValue, currentValue);
            yield return new WaitForSeconds(Mathf.Max(0.1f, statRegenTick));
        }
    }
    /// <summary>
    /// Hook for syncing the player data
    /// </summary>
    /// <param name="old"></param>
    /// <param name="newData"></param>
    void ChangePlayerDataForCallBacks(MobaPlayerData  old, MobaPlayerData newData)
    {
        playerDataForCallbacks = newData;
    }
}
