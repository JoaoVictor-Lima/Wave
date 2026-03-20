using Assets.ScriptableObjects;
using Assets.Scripts.Gameplay.Items.Modules;
using Assets.Scripts.Gameplay.Items.Types;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Equipament
{
    public class CharacterEquipment
    {
        private Dictionary<EquipmentSlotType, ItemInstance> equippedItems = new Dictionary<EquipmentSlotType, ItemInstance>();

        public event Action<EquipmentSlotType, ItemInstance> OnItemEquipped;
        public event Action<EquipmentSlotType> OnItemUnequipped;

        public bool Equip(ItemInstance item)
        {
            var equipmentModule = item.ItemData.GetModule<EquipmentModule>();

            if (equipmentModule == null)
                return false;

            EquipmentSlotType slot = equipmentModule.SlotType;

            equippedItems[slot] = item;

            OnItemEquipped?.Invoke(slot, item);

            return true;
        }

        public void Unequip(ItemInstance item)
        {
            var equipmentModule = item.ItemData.GetModule<EquipmentModule>();

            if (equipmentModule == null)
            {
                Debug.LogWarning($"Attempted to unequip an item that is not equippable: {item.ItemData.ItemName}");
            }

            EquipmentSlotType slot = equipmentModule.SlotType;

            if (equippedItems.Remove(slot))
            {
                OnItemUnequipped?.Invoke(slot);
            }
        }

        public ItemInstance GetItem(EquipmentSlotType slot)
        {
            equippedItems.TryGetValue(slot, out var item);
            return item;
        }
    }
}
