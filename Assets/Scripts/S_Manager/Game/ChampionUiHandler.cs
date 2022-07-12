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

        private bool _initialized;
        private Stat[] _stats;
        private MobaPlayerData _playerData;
        private GameObject _currentChampion;
    
        private readonly Dictionary<StatType, TMP_Text> _statValues = new();

        private void Update()
        {
            if (_playerData == null)
            {
                _playerData = NetworkClient.connection.identity.GetComponent<MobaPlayerData>();
            }

            if (_playerData != null && !_initialized)
            {
                InitializeChampionData();
                _initialized = true;
            }
        }

        private void UpdateUiForGivenStat(StatType statType,int currentValue,int maxValue)
        {
            _statValues[statType].text = $"{currentValue} / {maxValue}";
            if (statType == StatType.health)
            {
                healthBar.fillAmount = currentValue / maxValue;
            }
        
            else if (statType == StatType.mana)
            {
                manaBar.fillAmount = currentValue / maxValue;
            }
        }

        private void InitializeChampionData()
        {
            championSprite.sprite = _playerData.allChampionsAvailable[_playerData.CurrentChampion].ChampionSprite;
            Debug.Log(_playerData.currentlyPlayedChampion);
            _currentChampion = _playerData.currentlyPlayedChampion;
        
            if (_currentChampion != null)
            {
                _stats = _currentChampion.GetComponents<Stat>();
            }
        
            foreach (var stat in _stats)
            {
                if (!_statValues.ContainsKey(stat.StatType))
                {
                    stat.ClientOnStatUpdated += UpdateUiForGivenStat;

                    switch (stat.StatType)
                    {
                        case StatType.health:
                            _statValues.Add(stat.StatType,championCurrentHealthValue);
                            _statValues[stat.StatType].text = $"{stat.CurrentValue} / {stat.MaxValue}";
                            break;
                        case StatType.mana:
                            _statValues.Add(stat.StatType,championCurrentManaValue);
                            _statValues[stat.StatType].text = $"{stat.CurrentValue} / {stat.MaxValue}";
                            break; 
                        case StatType.attackValue:
                            _statValues.Add(stat.StatType,currentAttackValue);
                            _statValues[stat.StatType].text = $"{stat.CurrentValue}";
                            break; 
                        case StatType.defenseValue:
                            _statValues.Add(stat.StatType,currentDefenseValue);
                            _statValues[stat.StatType].text = $"{stat.CurrentValue}";
                            break;
                    }
                }
            }
        }

        private void OnDestroy()
        {
            foreach (var stat in _stats)
            {
                stat.ClientOnStatUpdated -= UpdateUiForGivenStat;
            }
        }
    }
}
