using S_Combat;
using S_Manager;
using UnityEngine;

namespace S_Unit
{
    public class UnitCommandGiver : MonoBehaviour
    {
        [SerializeField] private UnitSelectionHandler unitSelectionHandler;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask targetableLayer;

        private Camera _mainCamera;
        private bool _canControlUnits = true;

        private void Start()
        {
            _mainCamera = Camera.main;
            GameOverManager.OnGameOver += () => _canControlUnits = false;
        }

        private void Update()
        {
            if (InputManager.instance.ClickedRightMouseButton() && _canControlUnits)
            {
                SetMovePoint();
                SetAttackPoint();
            }
        }
        
        private void SetMovePoint()
        {
            Ray ray = _mainCamera.ScreenPointToRay(InputManager.instance.GetMousePos());
            
            bool validMovePos = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer);
          
            if (!validMovePos) return;


            foreach (var unitMovement in unitSelectionHandler.SelectedUnits)
            {
                unitMovement.GetUnitMovement().CmdMove(hit.point);
            }
        }

        void SetAttackPoint()
        {
            Ray ray = _mainCamera.ScreenPointToRay(InputManager.instance.GetMousePos());

            bool validAttackPos = Physics.Raycast(ray,out RaycastHit hit,Mathf.Infinity,targetableLayer);

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