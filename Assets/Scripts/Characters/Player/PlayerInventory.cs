using Assets.ScriptableObjects;
using Assets.Scripts.Gameplay.Inventory;
using System;
using UnityEngine;

namespace Assets.Scripts.Entities.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public InventoryBase Inventory { get; private set; }

        public event Action OnOpenRequested;
        public event Action OnCloseRequested;

        private void Awake()
        {
            Inventory = new InventoryBase();
            Inventory.SetSlots(54);
        }

        public void OpenInventory() => OnOpenRequested?.Invoke();
        public void CloseInventory() => OnCloseRequested?.Invoke();

        public void AddItem(ItemData itemData, int amount)
        {
            if (itemData.Stackable)
            {
                foreach (var item in Inventory.Items)
                {
                    if (item.ItemData == itemData && item.Quantity < itemData.MaxStack)
                    {
                        int addAmount = Mathf.Min(amount, itemData.MaxStack - item.Quantity);
                        item.Quantity += addAmount;
                        amount -= addAmount;

                        Inventory.NotifyChanged();

                        if (amount <= 0)
                            return;
                    }
                }
            }

            while (amount > 0)
            {
                int stack = itemData.Stackable ? Mathf.Min(amount, itemData.MaxStack) : 1;
                Inventory.AddItem(new ItemInstance(itemData, stack));
                amount -= stack;
            }
        }
    }
}
