using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Combat;
using UnityEngine;

public class RangedAttack : Attack
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform attackPoint;
    [SerializeField] float projectileSpeed;
    protected override IEnumerator AttackTarget(IDamageable target)
    {
        canAttack = false;
        Debug.Log("HEYRANGED");
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

        InitializeProjectile();

        yield return new WaitForSeconds(timeBeetweenAttacks);
        canAttack = true;
    }

    void InitializeProjectile()
    {
        var tempProjcetlie = Instantiate(projectile, attackPoint.position, Quaternion.identity);
        tempProjcetlie.transform.forward = attackPoint.transform.forward;
        var projectileInstance = tempProjcetlie.GetComponent<BaseAttackProjectile>();
        projectileInstance.Initialize(transform, targeter.GetTarget(), dmg, projectileSpeed);
        NetworkServer.Spawn(tempProjcetlie, connectionToClient);
    }
}