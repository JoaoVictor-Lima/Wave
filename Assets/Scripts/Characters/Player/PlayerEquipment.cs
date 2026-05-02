using Assets.ScriptableObjects;
using Assets.Scripts.Gameplay.Equipament;
using Assets.Scripts.Gameplay.Items.Modules;
using Assets.Scripts.Gameplay.Items.Types;
using UnityEngine;

namespace Assets.Scripts.Characters.Player
{
    public class PlayerEquipment : MonoBehaviour
    {
        public CharacterEquipment Equipment = new CharacterEquipment();

        [SerializeField] private Transform handR;

        private PlayerCombat playerCombat;
        private GameObject currentWeapon;

        private void Start()
        {
            playerCombat = GetComponent<PlayerCombat>();

            Equipment.OnItemEquipped += HandleItemEquipped;
            Equipment.OnItemUnequipped += HandleItemUnequipped;
        }

        private void HandleItemEquipped(EquipmentSlotType slot, ItemInstance item)
        {
            var visualModule = item.ItemData.GetModule<WorldVisualItemModule>();

            if (visualModule == null)
                return;

            if (slot == EquipmentSlotType.Weapon)
            {
                if (currentWeapon != null)
                    Destroy(currentWeapon);

                var weapon = ItemAssembly.Assemble(item, handR);
                currentWeapon = weapon;

                if (playerCombat != null)
                    playerCombat.SetWeapon(weapon.GetComponent<WeaponCombat>());
            }
        }

        private void HandleItemUnequipped(EquipmentSlotType slot)
        {
            if (slot != EquipmentSlotType.Weapon)
                return;

            if (playerCombat != null)
                playerCombat.SetWeapon(null);

            if (currentWeapon != null)
            {
                Destroy(currentWeapon);
                currentWeapon = null;
            }
        }
    }
}
