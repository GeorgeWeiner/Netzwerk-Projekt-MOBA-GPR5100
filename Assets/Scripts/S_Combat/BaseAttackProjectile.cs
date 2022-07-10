using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Combat;
using UnityEngine;

public class BaseAttackProjectile : Projectile
{
    private Targetable _target;

    void Update()
    {
        MoveTowardsTarget();
    }
    public override void Initialize(Transform forward, Targetable target, int dmg, float projectileSpeed)
    {
        this.dmg = dmg;
        this._target = target;
        this.projectileSpeed = projectileSpeed;
        transform.forward = forward.forward;
    }
    void MoveTowardsTarget()
    {
        if (_target == null)
        {
            Destroy(gameObject);
        }
        transform.position = Vector3.MoveTowards(transform.position, _target.GetAimAtPoint().position, Time.deltaTime * projectileSpeed);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Targetable>(out var target))
        { 
            if (target == this._target)
            {
                target.GetComponent<IDamageable>().TakeDmg(dmg);
                Destroy(gameObject);
            }
        }
    }
}
