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
    public int statRegenAmount;
    public float statRegenTick;
    public event Action<StatType, int,int> ClientOnStatUpdated;
    [SyncVar(hook = nameof(ChangePlayerDataForCallBacks))]public MobaPlayerData playerDataForCallbacks;

    void Start()
    {
        currentValue = maxValue;
        StartCoroutine(PassiveStatRegen());
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

    protected virtual IEnumerator PassiveStatRegen()
    {
        while (true)
        {
            currentValue += statRegenAmount;
            currentValue = Mathf.Min(maxValue, currentValue);
            yield return new WaitForSeconds(Mathf.Max(0.1f, statRegenTick));
        }
    }

    void ChangePlayerDataForCallBacks(MobaPlayerData  old, MobaPlayerData newData)
    {
        playerDataForCallbacks = newData;
    }
}
