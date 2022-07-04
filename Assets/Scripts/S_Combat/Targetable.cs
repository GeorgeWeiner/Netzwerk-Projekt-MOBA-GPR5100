using Mirror;
using UnityEngine;

namespace S_Combat
{
    public class Targetable : NetworkBehaviour
    {
        [SerializeField] private Transform aimAtPoint;

        public Transform GetAimAtPoint()
        {
            return aimAtPoint;
        }
    }
}