using System.Collections;
using System.Collections.Generic;
using S_Combat;
using UnityEngine;

public class MeleeAttack : Attack
{
    protected override IEnumerator AttackTarget(IDamageable damageTarget)
    {
        canAttack = false;
        float attackDelay = this.attackDelay;

        while (attackDelay > 0)
        {
            attackDelay -= Time.deltaTime;
            if (hasAuthority && InputManager.instance.ClickedRightMouseButton())
            {
                canAttack = true;
            }
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Attack");
        damageTarget.TakeDmg(4);

        yield return new WaitForSeconds(attackCd);
        canAttack = true;
    }
}
