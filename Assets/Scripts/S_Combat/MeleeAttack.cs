using System;
using System.Collections;
using System.Collections.Generic;
using S_Combat;
using UnityEngine;

public class MeleeAttack : Attack
{
    protected override IEnumerator AttackTarget(IDamageable damageTarget)
    {
        canAttack = false;
        float attackDelay = base.attackDelay;

        while (attackDelay > 0)
        {
            attackDelay -= Time.deltaTime;
            yield return new WaitForEndOfFrame();

            if (InputManager.instance.ClickedRightMouseButton())
            {
                canAttack = true;
                StopCoroutine(attackRoutine);
                attackRoutine = null;
            }

            yield return new WaitForEndOfFrame();
        }
        
        damageTarget.TakeDmg(4);

        yield return new WaitForSeconds(timeBeetweenAttacks);
        canAttack = true;
    }
}
