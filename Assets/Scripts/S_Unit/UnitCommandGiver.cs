using S_Combat;
using UnityEngine;

namespace S_Unit
{
    public class UnitCommandGiver : MonoBehaviour
    {
        [SerializeField] private UnitSelectionHandler unitSelectionHandler;
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


            foreach (var unitMovement in unitSelectionHandler.SelectedUnits)
            {
                unitMovement.GetUnitMovement().CmdMove(hit.point);
            }
        }
    }
}