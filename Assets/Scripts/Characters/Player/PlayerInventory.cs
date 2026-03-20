using Assets.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Entities.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public Inventory Inventory { get; private set; }
        [SerializeField] private InventoryUIController InventoryUi;

        private void Awake()
        {
            Inventory = new Inventory();
            Inventory.OnInventoryChanged += InventoryUi.RefreshUI;
        }

        public void OpenInventory()
        {
            InventoryUi.Open(Inventory);
        }

        public void CloseInventory()
        {
            InventoryUi.Close();
        }

        public void AddItem(ItemData itemData, int amount)
        {
            Debug.Log($"Adding {amount} of {itemData.ItemName} to inventory.");
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
