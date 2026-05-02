using Assets.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Inventory
{
    public class InventoryBase
    {
        private int qtdSlots = 54;
        private InventorySlot[] slots;

        public InventoryBase()
        {
            Items = new List<ItemInstance>();
            SetSlots(qtdSlots);
        }

        public InventoryBase(int slots)
        {
            Items = new List<ItemInstance>();
            SetSlots(slots);
        }

        public List<ItemInstance> Items { get; private set; }
        public event Action OnInventoryChanged;

        public void SetSlots(int newSlots)
        {
            if (newSlots < Items.Count)
            {
                Debug.LogWarning("Cannot reduce inventory slots below current item count.");
                return;
            }
            qtdSlots = newSlots;
            slots = new InventorySlot[qtdSlots];

            for (int i = 0; i < slots.Length; i++)
            {
                slots[i] = new InventorySlot();
            }

            OnInventoryChanged?.Invoke();
        }

        public int GetQtdSlots() => qtdSlots;

        private int? GetFirstIndexEmptySlot()
        {
            for (int i = 0; i < qtdSlots; i++)
            {
                var slot = slots[i];

                if (slot.ItemId == null || slots[i].ItemId == Guid.Empty)
                {
                    return i;
                }
            }

            return null; // No empty slots available
        }

        public InventorySlot? GetSlotByIndex(int index)
        {
            if (index < 0 || index >= qtdSlots) return null;
            return slots[index];
        }

        public void AddItem(ItemInstance item)
        {
            if (item.SlotIndex.HasValue)
            {
                var slot = GetSlotByIndex(item.SlotIndex.Value);

                if (slot.ItemId != null)
                {
                    if (slot.ItemId != item.Id)
                    {
                        Debug.Log("slot has an item");
                        return; // Slot is occupied by a different item, cannot add
                    }

                    slot.SetItem(item);
                }

                slot.SetItem(item);
            }
            else
            {
                var slotIndex = GetFirstIndexEmptySlot();
                if (!slotIndex.HasValue)
                {
                    return; // No empty slots available, cannot add item
                }

                var slot = GetSlotByIndex(slotIndex.Value);

                slot.SetItem(item);
                item.SlotIndex = slotIndex.Value;
            }

            Items.Add(item);
            OnInventoryChanged?.Invoke();
        }

        public void RemoveItem(ItemInstance item)
        {
            Items.Remove(item);
            OnInventoryChanged?.Invoke();
        }
        public void NotifyChanged()
        {
            Debug.Log("Inventory changed, notifying listeners...");
            OnInventoryChanged?.Invoke();
        }
    }
}
