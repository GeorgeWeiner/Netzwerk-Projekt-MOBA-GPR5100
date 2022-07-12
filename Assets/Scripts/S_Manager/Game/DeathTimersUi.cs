using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathTimersUi : NetworkBehaviour
{
    [SerializeField] Sprite Tes;
    [SerializeField] Image[] playerDeathCounterImages;
    [SerializeField] Image[] PlayerChampionSprite;
    [SerializeField] TMP_Text[] playerDeathTimers;

    void Start()
    {
        DeathCounterImages();
    }
    /// <summary>
    /// Sets the deathcounterimages in order and sets the deathcounter image
    /// </summary>
    /// <param name="player"></param>
    /// <param name="playerData"></param>
    void DeathCounterImages()
    {
        int i = 0;
        foreach (var instanceDeathTimer in GameManager.Instance.deathTimers)
        {
            PlayerChampionSprite[i].sprite = instanceDeathTimer.Key.allChampionsAvailable[instanceDeathTimer.Key.CurrentChampion].ChampionSprite;
            instanceDeathTimer.Value.deathTimer = playerDeathTimers[i];
            i++;
        }
    }
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
               Debug.Log(i);
               return;
            }
            i++;
        }
    }
}
