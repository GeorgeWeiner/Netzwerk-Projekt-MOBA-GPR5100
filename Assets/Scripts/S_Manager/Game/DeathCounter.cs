using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A simple death counter which displays how long youre dead and when the player on revive should be invoked
/// </summary>
public class DeathCounter : NetworkBehaviour
{
    public TMP_Text deathTimer;
    [SyncVar(hook = nameof(SyncDeathTime))]public float deathTime;
    public bool isDead;
    /// <summary>
    /// Invokes Revive player after the timer ran out and deactivates the death timer for the player
    /// </summary>
    /// <param name="deathTimeToAdd"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    public IEnumerator StartDeathCountdown(float deathTimeToAdd,MobaPlayerData player)
    {
        DeathTimersUi.instance.ActivateDeathCounterUI(player);

        float initialDeathTime = deathTime;
      
        while (deathTime > 0)
        {
            deathTime -= Time.deltaTime;
            deathTimer.text = Mathf.RoundToInt(deathTime).ToString();
            isDead = true;
            yield return new WaitForEndOfFrame();
        }
        DeathTimersUi.instance.DeActivateDeathCounterUI(player);
        GameManager.Instance.RevivePlayer(player);

        deathTime += initialDeathTime + deathTimeToAdd;
        isDead = false;
        
    }
    /// <summary>
    /// Updates the death timer text for both clients
    /// </summary>
    /// <param name="old"></param>
    /// <param name="newValue"></param>
    void SyncDeathTime(float old,float newValue)
    {
        deathTime = newValue;
        deathTimer.text = Mathf.RoundToInt(deathTime).ToString();
    }
}
