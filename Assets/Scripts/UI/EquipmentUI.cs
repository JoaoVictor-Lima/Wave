using Assets.ScriptableObjects;
using Assets.Scripts.Gameplay.Equipament;
using Assets.Scripts.Gameplay.Items.Types;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class EquipmentUI : MonoBehaviour
    {
        [SerializeField] private List<EquipamentSlot> slots;

        private Dictionary<EquipmentSlotType, EquipamentSlot> slotMap;

        public void Initialize(CharacterEquipment equipment)
        {
            slotMap = new Dictionary<EquipmentSlotType, EquipamentSlot>();

            foreach (var slot in slots)
            {
                slotMap[slot.SlotType] = slot;
            }

            equipment.OnItemEquipped += OnItemEquipped;
            equipment.OnItemUnequipped += OnItemUnequipped;
        }

        private void OnItemEquipped(EquipmentSlotType slot, ItemInstance item)
        {
            Debug.Log($"Item equipped in slot {slot}: {item.ItemData.ItemName}");
            if (slotMap.TryGetValue(slot, out var uiSlot))
            {
                uiSlot.SetItem(item);
            }
        }

        private void OnItemUnequipped(EquipmentSlotType slot)
        {
            if (slotMap.TryGetValue(slot, out var uiSlot))
            {
                uiSlot.Clear();
            }
        }
    }
}
