using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathCounter : NetworkBehaviour
{
    [SerializeField][SyncVar(hook = nameof(UpdateDeathTimersUI))] DeathTimersUi deathTimerUi;
    public TMP_Text deathTimer;
    [SyncVar(hook = nameof(SyncDeathTime))]public float deathTime;
    public bool isDead;

    public IEnumerator StartDeathCountdown(float deathTimeToAdd,MobaPlayerData player)
    {
        if (deathTimerUi == null)
        {
            deathTimerUi = FindObjectOfType<DeathTimersUi>();
        }
        deathTimerUi.ActivateDeathCounterUI(player);
        float initialDeathTime = deathTime;
        if (deathTimerUi == null)
        {
            deathTimerUi = FindObjectOfType<DeathTimersUi>();
        }
        while (deathTime > 0)
        {
            deathTime -= Time.deltaTime;
            deathTimer.text = Mathf.RoundToInt(deathTime).ToString();
            isDead = true;
            yield return new WaitForEndOfFrame();
        }

        deathTime += deathTimeToAdd;
        isDead = false;
        
    }
    void SyncDeathTime(float old,float newValue)
    {
        deathTime = newValue;
        deathTimer.text = Mathf.RoundToInt(deathTime).ToString();
    }

    void UpdateDeathTimersUI(DeathTimersUi old, DeathTimersUi newDeathTimer)
    {
        deathTimerUi = newDeathTimer;
    }
}
