using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace S_Manager.Game
{
    public class ChampionUiHandler : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private Image manaBar;
        [SerializeField] private Image championSprite;
        [SerializeField] private TMP_Text championCurrentHealthValue;
        [SerializeField] private TMP_Text championCurrentManaValue;
        [SerializeField] private TMP_Text currentAttackValue;
        [SerializeField] private TMP_Text currentDefenseValue;

        private bool initialized;
        private Stat[] stats;
        private MobaPlayerData playerData;
        private GameObject currentChampion;

        private readonly Dictionary<StatType, TMP_Text> statValues = new();

        private void Update()
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
        private void UpdateUiForGivenStat(StatType statType, int currentValue, int maxValue)
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
                            statValues.Add(stat.StatType, championCurrentHealthValue);
                            statValues[stat.StatType].text = $"{stat.CurrentValue} / {stat.MaxValue}";
                            break;
                        case StatType.mana:
                            statValues.Add(stat.StatType, championCurrentManaValue);
                            statValues[stat.StatType].text = $"{stat.CurrentValue} / {stat.MaxValue}";
                            break;
                        case StatType.attackValue:
                            statValues.Add(stat.StatType, currentAttackValue);
                            statValues[stat.StatType].text = $"{stat.CurrentValue}";
                            break;
                        case StatType.defenseValue:
                            statValues.Add(stat.StatType, currentDefenseValue);
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
}
