using Assets.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.World
{
    public class ItemEntity : Entity
    {
        [SerializeField] private ItemInstance itemInstance;
        private Rigidbody rb;

        public void Initialize(ItemInstance instance)
        {
            itemInstance = instance;
        }

        public ItemInstance GetItem()
        {
            return itemInstance;
        }

        public void Pickup(Inventory inventory)
        {
            inventory.AddItem(itemInstance);
            Destroy(gameObject);
        }
    }
}