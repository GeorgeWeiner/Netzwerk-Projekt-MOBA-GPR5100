using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// handles the deathtimers visually
/// </summary>
public class DeathTimersUi : BaseNetworkBehaviourSingleton<DeathTimersUi>
{
    [SerializeField] Image[] playerDeathCounterImages;
    [SerializeField] Image[] PlayerChampionSprite;
    [SerializeField] TMP_Text[] playerDeathTimers;

    void Start()
    {
        InitializeDeathCounterImages();
    }
    /// <summary>
    /// Sets the deathcounter images in order and sets the deathcounter image for each player with their champion
    /// </summary>
    /// <param name="player"></param>
    /// <param name="playerData"></param>
    void InitializeDeathCounterImages()
    {
        int i = 0;
        foreach (var instanceDeathTimer in GameManager.Instance.deathTimers)
        {
            PlayerChampionSprite[i].sprite = instanceDeathTimer.Key.allChampionsAvailable[instanceDeathTimer.Key.CurrentChampion].ChampionSprite;
            instanceDeathTimer.Value.deathTimer = playerDeathTimers[i];
            i++;
        }
    }
    /// <summary>
    /// Activates the death counter of a given player for all players
    /// </summary>
    /// <param name="player"></param>
    [ClientRpc]
    public void ActivateDeathCounterUI(MobaPlayerData player)
    {
        int i = 0;
        foreach (var instanceDeathTimer in GameManager.Instance.deathTimers)
        {
            if (instanceDeathTimer.Key == player)
            {
               playerDeathCounterImages[i].gameObject.SetActive(true);
               playerDeathTimers[i].gameObject.SetActive(true);
               return;
            }
            i++;
        }
    }
    /// <summary>
    /// Rpc for deactivating the death counter ui on all clients
    /// </summary>
    /// <param name="player"></param>
    [ClientRpc]
    public void DeActivateDeathCounterUI(MobaPlayerData player)
    {
        int i = 0;
        foreach (var instanceDeathTimer in GameManager.Instance.deathTimers)
        {
            if (instanceDeathTimer.Key == player)
            {
                playerDeathCounterImages[i].gameObject.SetActive(false);
                playerDeathTimers[i].gameObject.SetActive(false);
                return;
            }
            i++;
        }
    }
}
