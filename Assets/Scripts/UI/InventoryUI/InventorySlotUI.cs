using Assets.ScriptableObjects;
using Assets.Scripts.UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class InventorySlotUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] private GameObject inventoryItemPrefab;

        public int SlotIndex { get; private set; }
        public InventoryItemUI CurrentItem { get; private set; }

        private ItemContextMenu _contextMenu;

        public void Bind(int index)
        {
            SlotIndex = index;
        }

        public void SetItem(ItemInstance item, ItemContextMenu contextMenu = null)
        {
            Clear();
            _contextMenu = contextMenu;

            var go = Instantiate(inventoryItemPrefab, transform);
            CurrentItem = go.GetComponent<InventoryItemUI>();
            CurrentItem.Initialize(item, contextMenu);
        }

        public void Clear()
        {
            if (CurrentItem != null)
            {
                Destroy(CurrentItem.gameObject);
                CurrentItem = null;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            var draggedItem = eventData.pointerDrag?.GetComponent<InventoryItemUI>();
            if (draggedItem == null) return;

            var sourceSlot = draggedItem.OriginalSlot;

            if (CurrentItem != null && sourceSlot != null)
            {
                var itemToSwap = CurrentItem;
                CurrentItem = null;
                sourceSlot.RemoveReference();
                sourceSlot.PlaceItem(itemToSwap);

                itemToSwap.ItemInstance.SlotIndex = sourceSlot.SlotIndex;
            }
            else if (sourceSlot != null)
            {
                sourceSlot.RemoveReference();
            }

            PlaceItem(draggedItem);
            draggedItem.NotifyDropped();

            draggedItem.ItemInstance.SlotIndex = SlotIndex;
        }

        public void PlaceItem(InventoryItemUI item)
        {
            CurrentItem = item;
            item.transform.SetParent(transform, false);
            item.ResetRectTransform();
        }

        public void RemoveReference()
        {
            CurrentItem = null;
        }
    }
}
