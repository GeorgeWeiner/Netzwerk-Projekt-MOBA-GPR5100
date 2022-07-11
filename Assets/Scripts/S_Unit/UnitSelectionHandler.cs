using System.Collections.Generic;
using Mirror;
using S_Player;
using UnityEngine;

namespace S_Unit
{
    public class UnitSelectionHandler : MonoBehaviour
    {
        public List<Unit> selectedUnits = new List<Unit>();
        private MobaPlayer _mobaPlayer;

        private void Update()
        {
            if (_mobaPlayer == null)
            {
                _mobaPlayer = NetworkClient.connection.identity.GetComponent<MobaPlayer>();
            }
            
            selectedUnits = _mobaPlayer.GetMyUnits();
        }
    }
}
