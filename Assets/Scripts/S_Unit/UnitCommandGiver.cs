using System;
using S_Player;
using UnityEngine;

namespace S_Unit
{
    public class UnitCommandGiver : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private LayerMask groundLayer;
        
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (InputManager.instance.ClickedRightMouseButton())
            {
                SetMovePoint();
            }
        }
        
        private void SetMovePoint()
        {
            Ray ray = mainCamera.ScreenPointToRay(InputManager.instance.GetMousePos());
            
            bool validPos = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer);
            if (!validPos) return;
            
            playerMovement.CmdMove(hit.point);
        }
    }
}