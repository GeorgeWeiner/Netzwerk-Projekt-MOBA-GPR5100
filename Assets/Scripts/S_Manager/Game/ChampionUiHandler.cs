using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Combat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChampionUiHandler : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] Image manaBar;
    [SerializeField] Image championSprite;
    [SerializeField] TMP_Text championCurrentHealthValue;
    [SerializeField] TMP_Text championCurrentManaValue;
    [SerializeField] TMP_Text currentAttackValue;
    [SerializeField] TMP_Text currentDefenseValue;

    bool initialized = false;
    Stat[] stats;
    Dictionary<StatType, TMP_Text> statValues = new Dictionary<StatType, TMP_Text>();
    MobaPlayerData playerData;
    GameObject currentChampion;

    void Update()
    {
        if (playerData == null)
        {
            playerData = NetworkClient.connection.identity.GetComponent<MobaPlayerData>();
        }

        if (playerData != null && !initialized)
        {
            InitializeChampionData();
            initialized = true;
        }
    }

    void UpdateUiForGivenStat(StatType statType,int currentValue,int maxValue)
    {
        statValues[statType].text = $"{currentValue} / {maxValue}";
        if (statType == StatType.health)
        {
            healthBar.fillAmount = currentValue / maxValue;
        }
        else if (statType == StatType.mana)
        {
            manaBar.fillAmount = currentValue / maxValue;
        }
    }
    void InitializeChampionData()
    {
        championSprite.sprite = playerData.allChampionsAvailable[playerData.CurrentChampion].ChampionSprite;
        Debug.Log(playerData.currentlyPlayedChampion);
        currentChampion = playerData.currentlyPlayedChampion;
        if (currentChampion != null)
        {
            stats = currentChampion.GetComponents<Stat>();
        }
        foreach (var stat in stats)
        {
            if (!statValues.ContainsKey(stat.StatType))
            {
                stat.ClientOnStatUpdated += UpdateUiForGivenStat;

                switch (stat.StatType)
                {
                    case StatType.health:
                        statValues.Add(stat.StatType,championCurrentHealthValue);
                        statValues[stat.StatType].text = $"{stat.CurrentValue} / {stat.MaxValue}";
                        break;
                    case StatType.mana:
                        statValues.Add(stat.StatType,championCurrentManaValue);
                        statValues[stat.StatType].text = $"{stat.CurrentValue} / {stat.MaxValue}";
                        break; 
                    case StatType.attackValue:
                        statValues.Add(stat.StatType,currentAttackValue);
                        statValues[stat.StatType].text = $"{stat.CurrentValue}";
                        break; 
                    case StatType.defenseValue:
                        statValues.Add(stat.StatType,currentDefenseValue);
                        statValues[stat.StatType].text = $"{stat.CurrentValue}";
                        break;
                }
                
                
            }
        }
    }
    void OnDestroy()
    {
        foreach (var stat in stats)
        {
            stat.ClientOnStatUpdated -= UpdateUiForGivenStat;
        }
    }
}
