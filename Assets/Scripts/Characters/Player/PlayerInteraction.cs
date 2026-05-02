using Assets.Scripts.Gameplay.Interact;
using UnityEngine;

namespace Assets.Scripts.Entities.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private float interactDistance = 3f;
        [SerializeField] private LayerMask interactLayer;
        [SerializeField] private Camera playerCamera;

        public void TryInteract()
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
                return;

            var interactable = hit.collider.GetComponentInParent<IInteractable>();
            if (interactable == null) return;

            var context = new InteractionContext
            {
                Type = InteractionType.Direct,
                Power = 1f,
                Capabilities = System.Array.Empty<Capability>(),
                Source = gameObject
            };

            interactable.Interact(context);
        }

        private void OnDrawGizmos()
        {
            if (playerCamera == null) return;

            Gizmos.color = Color.yellow;
            Vector3 origin = playerCamera.transform.position;
            Gizmos.DrawLine(origin, origin + playerCamera.transform.forward * interactDistance);
        }
    }
}
