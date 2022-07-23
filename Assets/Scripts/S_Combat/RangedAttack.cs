using System.Collections;
using Mirror;
using UnityEngine;

public class RangedAttack : Attack
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform attackPoint;
   
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
    private void CmdInitializeProjectile()
    {
        MobaNetworkRoomManager.SpawnPrefab(projectile,attackPoint,null,dmg,projectileSpeed,targeter.GetTarget());
    }
}
