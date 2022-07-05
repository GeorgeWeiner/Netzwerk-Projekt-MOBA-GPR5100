using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

public abstract class Attack : NetworkBehaviour
{
    
    public void Start()
    {
     
       
    }
    void Update()
    {
       
    }
    void AttackTargetable()
    {
       
    }
    public void SetTargetable()
    {
      
    }
    protected abstract IEnumerator AttackTarget(IDamageable target);
    void OnDrawGizmosSelected()
    {
       
    }
}
