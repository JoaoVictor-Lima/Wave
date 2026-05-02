using Assets.ScriptableObjects;
using Assets.Scripts.Entities.Player;
using Assets.Scripts.Gameplay.Interact;
using Assets.Scripts.Gameplay.Inventory;
using UnityEngine;

namespace Assets.Scripts.World
{
    public class ItemEntity : Entity, IInteractable
    {
        [SerializeField] private ItemInstance itemInstance;

        public void Initialize(ItemInstance instance)
        {
            itemInstance = instance;
        }

        public ItemInstance GetItem() => itemInstance;

        public void Interact(InteractionContext context)
        {
            var playerInventory = context.Source.GetComponent<PlayerInventory>();
            if (playerInventory == null) return;

            Pickup(playerInventory.Inventory);
        }

        public void Pickup(InventoryBase inventory)
        {
            inventory.AddItem(itemInstance);
            Destroy(gameObject);
        }
    }
}
