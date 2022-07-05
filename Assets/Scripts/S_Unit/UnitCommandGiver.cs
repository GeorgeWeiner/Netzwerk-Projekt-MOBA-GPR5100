using S_Combat;
using UnityEngine;

namespace S_Unit
{
    public class UnitCommandGiver : MonoBehaviour
    {
        [SerializeField] private UnitSelectionHandler unitSelectionHandler;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] LayerMask playerLayer;

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
                SetAttackPoint();
            }
        }
        
        private void SetMovePoint()
        {
            Ray ray = mainCamera.ScreenPointToRay(InputManager.instance.GetMousePos());
            
            bool validMovePos = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer);
          
            if (!validMovePos) return;


            foreach (var unitMovement in unitSelectionHandler.SelectedUnits)
            {
                unitMovement.GetUnitMovement().CmdMove(hit.point);
            }
        }

        void SetAttackPoint()
        {
            Ray ray = mainCamera.ScreenPointToRay(InputManager.instance.GetMousePos());

            bool validAttackPos = Physics.Raycast(ray,out RaycastHit hit,Mathf.Infinity,playerLayer);

            if (!validAttackPos || !hit.collider.TryGetComponent<Targetable>(out Targetable target)) return;
            {
                foreach (var selectedUnit in unitSelectionHandler.SelectedUnits)
                {
                    if (!unitSelectionHandler.SelectedUnits.Contains(target.GetComponent<Unit>()))
                    {
                        selectedUnit.Targeter.CmdSetTarget(target.gameObject);
                        selectedUnit.GetUnitMovement().CmdMoveTowardsAttackTarget(target.transform.position);
                    }
                }
            }
        }
    }
}