using System.Collections.Generic;
using Mirror;
using S_Player;
using UnityEngine;

namespace S_Unit
{
    public class UnitSelectionHandler : MonoBehaviour
    {
        public List<Unit> SelectedUnits = new List<Unit>();
        private MobaPlayer mobaPlayer;

        private void Update()
        {
            if (mobaPlayer == null)
            {
                mobaPlayer = NetworkClient.connection.identity.GetComponent<MobaPlayer>();
            }
            
            SelectedUnits = mobaPlayer.GetMyUnits();
        }
    }
}
