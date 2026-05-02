using Assets.ScriptableObjects;
using System;

namespace Assets.Scripts.Gameplay.Inventory
{
    public class InventorySlot
    {
        public Guid? ItemId;

        public void SetItem(ItemInstance item)
        {
            ItemId = item.Id;
        }
    }
}
