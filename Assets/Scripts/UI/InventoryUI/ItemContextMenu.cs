using Assets.ScriptableObjects;
using Assets.Scripts.Characters.Player;
using Assets.Scripts.Gameplay.Items.Modules;
using Assets.Scripts.Gameplay.Items.Types;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public class ItemContextMenu : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private RectTransform optionsContainer;
        [SerializeField] private ItemContextButton buttonPrefab;

        private PlayerEquipment playerEquipment;
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Initialize(PlayerEquipment equipment)
        {
            playerEquipment = equipment;
        }

        public void Open(List<ItemOptionType> options, ItemInstance item, RectTransform itemRect)
        {
            ClearButtons();

            foreach (var option in options)
            {
                if (option == ItemOptionType.Unequip && !IsItemEquipped(item))
                    continue;

                if (option == ItemOptionType.Equip && IsItemEquipped(item))
                    continue;

                var button = Instantiate(buttonPrefab, optionsContainer);

                button.Setup(option, () =>
                {
                    ExecuteOption(option, item);
                });
            }

            gameObject.SetActive(true);
            PositionMenu();
        }

        private bool IsItemEquipped(ItemInstance item)
        {
            var equipmentModule = item.ItemData.GetModule<EquipmentModule>();

            if (equipmentModule == null)
                return false;

            var equippedItem = playerEquipment.Equipment.GetItem(equipmentModule.SlotType);

            return equippedItem == item;
        }

        public void PositionMenu()
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            Vector2 mousePos = Mouse.current.position.ReadValue();

            Vector2 localMousePos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                mousePos,
                canvas.worldCamera,
                out localMousePos
            );

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(optionsContainer);

            float menuWidth = rectTransform.rect.width;
            float menuHeight = rectTransform.rect.height;

            Vector2 pos = new Vector2(
                localMousePos.x - menuWidth,
                localMousePos.y
            );

            if (pos.x < canvasRect.rect.xMin)
                pos.x = localMousePos.x;

            if (pos.y - menuHeight < canvasRect.rect.yMin)
                pos.y = canvasRect.rect.yMin + menuHeight;

            rectTransform.anchoredPosition = pos;
        }

        public void Close()
        {
            ClearButtons();
            gameObject.SetActive(false);
        }

        private void ClearButtons()
        {
            foreach (Transform child in optionsContainer)
            {
                Destroy(child.gameObject);
            }
        }

        private void ExecuteOption(ItemOptionType option, ItemInstance item)
        {
            Debug.Log($"Executing {option} on {item}");

            switch (option)
            {
                case ItemOptionType.Equip:
                    EquipItem(item);
                    break;
                case ItemOptionType.Unequip:
                    UnequipItem(item);
                    break;
                case ItemOptionType.Consume:
                    break;
                case ItemOptionType.Drop:
                    break;
                case ItemOptionType.Inspect:
                    break;
                case ItemOptionType.Description:
                    break;
                default:
                    break;
            }

            Close();
        }

        private void EquipItem(ItemInstance item)
        {
            var equipmentModule = item.ItemData.GetModule<EquipmentModule>();

            if (equipmentModule == null)
            {
                Debug.Log("Item is not equipable");
                return;
            }

            bool equipped = playerEquipment.Equipment.Equip(item);

            if (equipped)
            {
                Debug.Log($"Equipped {item.ItemData.ItemName} in {equipmentModule.SlotType}");
            }
        }

        private void UnequipItem(ItemInstance item)
        {
            var equipmentModule = item.ItemData.GetModule<EquipmentModule>();

            if (equipmentModule == null)
            {
                Debug.Log("Item dont have an equipment module");
                return;
            }

            playerEquipment.Equipment.Unequip(item);
        }
    }
}
