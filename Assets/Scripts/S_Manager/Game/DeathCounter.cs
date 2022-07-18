using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathCounter : NetworkBehaviour
{
    public TMP_Text deathTimer;
    [SyncVar(hook = nameof(SyncDeathTime))]public float deathTime;
    public bool isDead;

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
    void SyncDeathTime(float old,float newValue)
    {
        deathTime = newValue;
        deathTimer.text = Mathf.RoundToInt(deathTime).ToString();
    }
}
