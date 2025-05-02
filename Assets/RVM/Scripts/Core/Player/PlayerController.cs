using RVM.Scripts.Core.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RVM.Scripts.Core.Player
{
    public class PlayerController : MonoBehaviour
    {
        public float interactDistance = 3f;
        public Camera mainCamera;
        public LayerMask interactLayer;

        private PlayerInputActions _inputActions;

        private void Awake()
        {
            _inputActions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            _inputActions.Enable();
            _inputActions.Player.Interact.performed += OnInteract;
        }

        private void OnDisable()
        {
            _inputActions.Player.Interact.performed -= OnInteract;
            _inputActions.Disable();
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            HandleInteract();
        }

        private void HandleInteract()
        {
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
            {
                if (hit.collider.TryGetComponent<IInteractable>(out var target))
                {
                    target.Interact();
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (mainCamera == null) return;

            Gizmos.color = Color.yellow;

            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            Gizmos.DrawRay(ray.origin, ray.direction * interactDistance);
        }


    }
}