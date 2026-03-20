using Assets.ScriptableObjects;
using Assets.Scripts.Gameplay.Items.Modules;
using Assets.Scripts.UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class InventoryItemUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Button button;
        [SerializeField] private Image icon;
        [SerializeField] private TMPro.TextMeshProUGUI itemName;
        [SerializeField] private TMPro.TextMeshProUGUI quantityText;
        [SerializeField] private TMPro.TextMeshProUGUI weightText;
        [SerializeField] private ItemContextMenu itemContextMenu;

        private ItemInstance itemInstance;

        public void Initialize(ItemInstance newItem)
        {
            icon.sprite = newItem.ItemData.Icon;
            itemName.text = newItem.ItemData.ItemName;
            quantityText.text = newItem.Quantity.ToString();
            itemInstance = newItem;
        }

        private void Awake()
        {
            itemContextMenu = GetComponentInParent<Canvas>().GetComponentInChildren<ItemContextMenu>(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Debug.Log($"Left click on {itemName.text}");
            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OpenContextMenu();
            }
        }

        public void OpenContextMenu()
        {
            var optionsModule = itemInstance.ItemData.GetModule<ItemOptionsModule>();

            if (optionsModule == null)
                return;

            itemContextMenu.Open(optionsModule.Options, itemInstance, GetComponent<RectTransform>());
        }
    }
}
