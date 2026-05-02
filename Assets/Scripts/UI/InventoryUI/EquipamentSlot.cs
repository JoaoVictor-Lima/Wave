using Assets.ScriptableObjects;
using Assets.Scripts.Gameplay.Items.Types;
using UnityEngine;
using UnityEngine.UI;

public class EquipamentSlot : MonoBehaviour
{
    [SerializeField] private EquipmentSlotType slotType;
    [SerializeField] private Image icon;

    private ItemInstance equippedItem;

    public EquipmentSlotType SlotType => slotType;

    public void SetItem(ItemInstance item)
    {
        equippedItem = item;
        icon.sprite = item.ItemData.Icon;
        icon.enabled = true;
    }

    public void Clear()
    {
        equippedItem = null;
        icon.enabled = false;
    }
}