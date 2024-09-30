using UnityEngine;
using UnityEngine.InputSystem;
using Movement.Provider;

namespace Movement.Adapter
{
    [RequireComponent(typeof(MovementProvider))]
    public class PointToMovementInputAdapter : MonoBehaviour
    {
        private Camera _mainCamera;
        private PointToMovementProvider _movementProvider;

        private void Start()
        {
        _mainCamera = Camera.main;
            _movementProvider = GetComponent<PointToMovementProvider>();
        }

        public void OnMove(InputValue value)
        {
            Debug.Log("HIHIHIH");
            // 마우스 위치에서 레이 발사
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                _movementProvider.MoveToPosition(hit.point);
            }
        }

        public void OnStop()
        {
            _movementProvider.Stop();
        }
    }
}