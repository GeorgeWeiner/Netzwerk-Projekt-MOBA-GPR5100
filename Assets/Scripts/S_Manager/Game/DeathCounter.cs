using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DeathCounter : NetworkBehaviour
{
    public float deathTime;
    public bool isDead;

    public IEnumerator StartDeathCountdown(float deathTimeToAdd,MobaPlayerData player)
    {
        float deathTime = this.deathTime;
        isDead = true;
        yield return new WaitForSeconds(deathTime);
        deathTime += deathTimeToAdd;
        isDead = false;
    }
}
