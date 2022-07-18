using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Combat;
using UnityEngine;

public class RangedAttack : Attack
{
    [SerializeField] float projectileSpeed;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform attackPoint;
   
    protected override IEnumerator AttackTarget(IDamageable target)
    {
        canAttack = false;
        float attackDelay = base.attackDelay;

        yield return new WaitForEndOfFrame();

        while (attackDelay > 0)
        {
            attackDelay -= Time.deltaTime;
            if (InputManager.instance.ClickedRightMouseButton())
            {
                canAttack = true;
                StopCoroutine(attackRoutine);
                attackRoutine = null;
            }
            yield return new WaitForEndOfFrame();
        }

        CmdInitializeProjectile();

        yield return new WaitForSeconds(timeBeetweenAttacks);
        canAttack = true;
    }
    [Command(requiresAuthority = false)]
    void CmdInitializeProjectile()
    {
        MobaNetworkRoomManager.SpawnPrefab(projectile,attackPoint,null,dmg,projectileSpeed,targeter.GetTarget());
    }
}
