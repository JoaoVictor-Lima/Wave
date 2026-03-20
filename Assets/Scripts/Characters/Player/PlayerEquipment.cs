using Assets.ScriptableObjects;
using Assets.Scripts.Gameplay.Equipament;
using Assets.Scripts.Gameplay.Items.Modules;
using Assets.Scripts.Gameplay.Items.Types;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Inventory;
using UnityEngine;

namespace Assets.Scripts.Characters.Player
{
    public class PlayerEquipment : MonoBehaviour
    {
        public CharacterEquipment Equipment = new CharacterEquipment();

        [SerializeField] private Transform handR;
        [SerializeField] private EquipmentUI equipmentUI;
        [SerializeField] private ItemContextMenu itemContextMenu; //TODO: Remove this reference and use an event system to communicate with the context menu

        private PlayerCombat playerCombat;
        private GameObject currentWeapon;

        void Start()
        {
            equipmentUI.Initialize(Equipment);
            playerCombat = GetComponent<PlayerCombat>();

            itemContextMenu.Initialize(this);

            Equipment.OnItemEquipped += HandleItemEquipped;
            Equipment.OnItemUnequipped += HandleItemUnequipped;
        }

        void HandleItemEquipped(EquipmentSlotType slot, ItemInstance item)
        {
            var visualModule = item.ItemData.GetModule<WorldVisualItemModule>();

            if (visualModule == null)
                return;

            if (slot == EquipmentSlotType.Weapon)
            {
                if (currentWeapon != null)
                {
                    Destroy(currentWeapon);
                }

                var weapon = ItemAssembly.Assemble(item, handR);
                currentWeapon = weapon;

                if (playerCombat != null)
                {
                    playerCombat.SetWeapon(weapon.GetComponent<WeaponCombat>());
                }
            }
        }

        void HandleItemUnequipped(EquipmentSlotType slot)
        {
            if (slot != EquipmentSlotType.Weapon)
                return;

            if (playerCombat != null)
            {
                playerCombat.SetWeapon(null);
            }

            if (currentWeapon != null)
            {
                Destroy(currentWeapon);
                currentWeapon = null;
            }
        }
    }
}
