using Mirror;
using UnityEngine;

namespace S_Combat
{
    public class Targeter : NetworkBehaviour
    { 
        public Targetable _target;
    
        [Command] 
        public void CmdSetTarget(GameObject targetGameObject)
        {
            if (!targetGameObject.TryGetComponent(out Targetable target)) return;
            this._target = target;
        }

        [Server]
        public void ClearTarget()
        {
            _target = null;
        }

        public Targetable GetTarget()
        {
            return _target;
        }
    }
}