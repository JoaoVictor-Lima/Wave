using Assets.ScriptableObjects;
using Assets.Scripts.Gameplay.Items.Modules;
using Assets.Scripts.UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class InventoryItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField] private Image icon;

        public ItemInstance ItemInstance { get; private set; }

        private ItemContextMenu contextMenu;
        private Canvas rootCanvas;
        private Transform originalParent;
        private Vector3 originalPosition;
        private bool wasDropped;

        public InventorySlotUI OriginalSlot { get; private set; }

        private void Awake()
        {
            rootCanvas = GetComponentInParent<Canvas>();
        }

        public void Initialize(ItemInstance item, ItemContextMenu contextMenu = null)
        {
            ItemInstance = item;
            this.contextMenu = contextMenu;
            icon.sprite = item.ItemData.Icon;
        }

        public void NotifyDropped()
        {
            wasDropped = true;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            wasDropped = false;
            rootCanvas = GetComponentInParent<Canvas>();
            OriginalSlot = GetComponentInParent<InventorySlotUI>();
            originalParent = transform.parent;
            originalPosition = transform.position;

            icon.raycastTarget = false;
            transform.SetParent(rootCanvas.transform, true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            icon.raycastTarget = true;

            if (!wasDropped)
            {
                transform.SetParent(originalParent, false);
                ResetRectTransform();
            }
        }

        // Reseta o RectTransform para preencher o slot pai corretamente
        public void ResetRectTransform()
        {
            var rt = GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.localScale = Vector3.one;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
                OpenContextMenu();
        }

        private void OpenContextMenu()
        {
            if (contextMenu == null) return;

            var optionsModule = ItemInstance?.ItemData.GetModule<ItemOptionsModule>();
            if (optionsModule == null) return;

            contextMenu.Open(optionsModule.Options, ItemInstance, GetComponent<RectTransform>());
        }
    }
}
