using Assets.Scripts.Gameplay.Inventory;
using Assets.Scripts.UI.Inventory;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class InventoryGridUI : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private GameObject slotPrefab;

        private InventorySlotUI[] _slots;
        private InventoryBase _inventory;
        private ItemContextMenu _contextMenu;

        public void Bind(InventoryBase inventory, ItemContextMenu contextMenu = null)
        {
            Unbind();

            _inventory = inventory;
            _contextMenu = contextMenu;

            BuildSlots();
            _inventory.OnInventoryChanged += Refresh;
            Refresh();
        }

        public void Unbind()
        {
            if (_inventory != null)
                _inventory.OnInventoryChanged -= Refresh;

            _inventory = null;
            _contextMenu = null;
            ClearSlots();
        }

        private void BuildSlots()
        {
            ClearSlots();

            _slots = new InventorySlotUI[_inventory.GetQtdSlots()];

            for (int i = 0; i < _inventory.GetQtdSlots(); i++)
            {
                var go = Instantiate(slotPrefab, container);
                var ui = go.GetComponent<InventorySlotUI>();
                ui.Bind(i);
                _slots[i] = ui;
            }
        }

        private void ClearSlots()
        {
            if (_slots == null) return;

            foreach (var slot in _slots)
                if (slot != null) Destroy(slot.gameObject);

            _slots = null;
        }

        public void HardClearSlots()
        {
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
        }

        private void Refresh()
        {
            if (_inventory == null || _slots == null) return;

            foreach (var slot in _slots)
                slot.Clear();

            foreach (var item in _inventory.Items)
            {
                if (!item.SlotIndex.HasValue) continue;
                _slots[item.SlotIndex.Value].SetItem(item, _contextMenu);
            }
        }

        private void OnDestroy() => Unbind();
    }
}
