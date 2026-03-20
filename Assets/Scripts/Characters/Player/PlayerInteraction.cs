using Assets.Scripts.World;
using UnityEngine;

namespace Assets.Scripts.Entities.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private float InteractDistance = 3f;
        [SerializeField] private LayerMask InteractLayer;
        [SerializeField] private Camera PlayerCamera;

        private PlayerInventory Inventory;

        private void Awake()
        {
            Inventory = GetComponent<PlayerInventory>();
        }

        public void TryInteract()
        {
            Ray ray = new Ray(
                PlayerCamera.transform.position,
                PlayerCamera.transform.forward
            );

            Debug.Log("Trying to interact...");

            if (Physics.Raycast(ray, out RaycastHit hit, InteractDistance, InteractLayer))
            {
                ItemEntity itemEntity = hit.collider.GetComponent<ItemEntity>();

                Debug.Log("Hit: " + hit.collider.name);


                if (itemEntity != null)
                {
                    Debug.Log("Interacted with: " + itemEntity.GetItem().ItemData.name);

                    itemEntity.Pickup(Inventory.Inventory);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (PlayerCamera == null) return;

            Gizmos.color = Color.yellow;

            Vector3 origin = PlayerCamera.transform.position;
            Vector3 direction = PlayerCamera.transform.forward;

            Gizmos.DrawLine(origin, origin + direction * InteractDistance);
        }
    }
}
