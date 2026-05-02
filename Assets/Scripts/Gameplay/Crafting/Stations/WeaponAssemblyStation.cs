using Assets.ScriptableObjects;
using Assets.Scripts.Entities.Player;
using Assets.Scripts.Gameplay.Interact;
using Assets.Scripts.Gameplay.Items;
using Assets.Scripts.Gameplay.Items.Modules;
using Assets.Scripts.Gameplay.Items.Weapon;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Crafting
{
    /// <summary>
    /// Mesa de montagem de armas: o jogador preenche os slots com partes
    /// (Blade, Handle, Guard, Pommel, etc.) e clica em "Forjar Arma".
    ///
    /// Validacao de compatibilidade usa WeaponPartShapeModule.CompatibleFamilies:
    /// as partes sao compativeis se existe ao menos uma WeaponFamily em comum entre todas elas.
    ///
    /// Fluxo:
    ///   1. UI chama SelectConfig() → slots sao reconstruidos pelos RequiredPartTypes
    ///   2. UI coloca ItemInstances nos slots
    ///   3. UI chama CanCraft() para habilitar o botao
    ///   4. UI chama Craft() → retorna a arma montada como ItemInstance
    ///   5. UI adiciona a arma ao inventario e consome as partes
    /// </summary>
    [AddComponentMenu("Crafting/Weapon Assembly Station")]
    public class WeaponAssemblyStation : MonoBehaviour, ICraftingStation, IInteractable
    {
        [SerializeField] private WeaponAssemblyConfig[] availableConfigs;

        private WeaponAssemblyConfig _selectedConfig;
        private readonly List<CraftingSlot> _slots = new();

        public IReadOnlyList<CraftingSlot> Slots => _slots;
        public IReadOnlyList<WeaponAssemblyConfig> AvailableConfigs => availableConfigs;

        public event Action OnSlotsChanged;

        public void SelectConfig(WeaponAssemblyConfig config)
        {
            if (config != null && !availableConfigs.Contains(config))
            {
                Debug.LogWarning($"Config '{config.ConfigName}' nao pertence a esta estacao.");
                return;
            }

            _selectedConfig = config;
            RebuildSlots();
            OnSlotsChanged?.Invoke();
        }

        private void RebuildSlots()
        {
            _slots.Clear();

            if (_selectedConfig == null) return;

            foreach (var partType in _selectedConfig.RequiredPartTypes)
            {
                _slots.Add(new CraftingSlot
                {
                    Label = partType.ToString(),
                    Filter = new WeaponPartTypeFilter { PartType = partType }
                });
            }
        }

        public bool CanCraft()
        {
            if (_selectedConfig == null) return false;
            if (_slots.Any(s => !s.IsOptional && s.IsEmpty)) return false;
            if (!ArePartsCompatible()) return false;

            return true;
        }

        public CraftingResult Craft()
        {
            if (!CanCraft())
                return CraftingResult.Fail("Partes incompativeis ou slots incompletos.");

            var componentItems = _slots
                .Where(s => s.IsFilled)
                .Select(s => s.CurrentItem.ItemData)
                .ToList();

            // Consome as partes (limpa os slots)
            foreach (var slot in _slots)
                slot.Clear();

            var weapon = ItemInstanceFactory.Create(_selectedConfig.WeaponData, 1, componentItems);
            return CraftingResult.Ok(new List<ItemInstance> { weapon });
        }

        /// <summary>
        /// Verifica se existe ao menos uma WeaponFamily compartilhada entre todas as partes colocadas.
        /// </summary>
        public void Interact(InteractionContext context)
        {
            var playerInventory = context.Source.GetComponent<PlayerInventory>();

            if (playerInventory == null)
            {
                Debug.LogWarning($"{gameObject.name}: quem interagiu nao tem PlayerInventory.");
                return;
            }

            StationEvents.NotifyOpened(this, playerInventory.Inventory);
        }

        private bool ArePartsCompatible()
        {
            HashSet<WeaponFamily> commonFamilies = null;

            foreach (var slot in _slots.Where(s => s.IsFilled))
            {
                var module = slot.CurrentItem.ItemData.GetModule<WeaponPartShapeModule>();
                if (module == null) continue;

                if (commonFamilies == null)
                    commonFamilies = new HashSet<WeaponFamily>(module.CompatibleFamilies);
                else
                    commonFamilies.IntersectWith(module.CompatibleFamilies);
            }

            return commonFamilies != null && commonFamilies.Count > 0;
        }
    }
}
