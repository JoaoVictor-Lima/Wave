using Assets.Scripts.Entities.Player;
using Assets.Scripts.Gameplay.Interact;
using Assets.Scripts.Gameplay.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Crafting
{
    /// <summary>
    /// Base para estacoes que funcionam por receita: Forja, Bigorna e Mesa de Crafting.
    /// A diferenca entre elas e apenas o conjunto de receitas configurado no Inspector.
    ///
    /// Fluxo:
    ///   1. UI chama SelectRecipe() → slots sao reconstruidos
    ///   2. UI coloca ItemInstances nos slots (vindos do inventario)
    ///   3. UI chama CanCraft() para habilitar o botao
    ///   4. UI chama Craft() → consome itens dos slots, retorna outputs
    ///   5. UI adiciona outputs ao inventario; devolve sobras dos slots se houver
    /// </summary>
    public abstract class RecipeBasedStation : MonoBehaviour, ICraftingStation, IInteractable
    {
        [SerializeField] private CraftingRecipe[] availableRecipes;

        private CraftingRecipe _selectedRecipe;
        private readonly List<CraftingSlot> _slots = new();

        public IReadOnlyList<CraftingSlot> Slots => _slots;
        public IReadOnlyList<CraftingRecipe> AvailableRecipes => availableRecipes;

        public event Action OnSlotsChanged;

        public void SelectRecipe(CraftingRecipe recipe)
        {
            if (recipe != null && !availableRecipes.Contains(recipe))
            {
                Debug.LogWarning($"Recipe '{recipe.RecipeName}' nao pertence a esta estacao.");
                return;
            }

            _selectedRecipe = recipe;
            RebuildSlots();
            OnSlotsChanged?.Invoke();
        }

        private void RebuildSlots()
        {
            _slots.Clear();

            if (_selectedRecipe == null) return;

            foreach (var ingredient in _selectedRecipe.Inputs)
            {
                _slots.Add(new CraftingSlot
                {
                    Label = $"{ingredient.ItemData.ItemName} x{ingredient.Quantity}",
                    Filter = new ItemDataFilter { Required = ingredient.ItemData }
                });
            }
        }

        public bool CanCraft()
        {
            if (_selectedRecipe == null) return false;

            for (int i = 0; i < _selectedRecipe.Inputs.Count; i++)
            {
                var slot = _slots[i];
                var ingredient = _selectedRecipe.Inputs[i];

                if (slot.IsEmpty) return false;
                if (slot.CurrentItem.Quantity < ingredient.Quantity) return false;
            }

            return true;
        }

        public CraftingResult Craft()
        {
            if (!CanCraft())
                return CraftingResult.Fail("Requisitos nao atendidos.");

            // Consome os itens dos slots
            for (int i = 0; i < _selectedRecipe.Inputs.Count; i++)
            {
                var slot = _slots[i];
                var ingredient = _selectedRecipe.Inputs[i];

                slot.CurrentItem.Quantity -= ingredient.Quantity;

                if (slot.CurrentItem.Quantity <= 0)
                    slot.Clear();
            }

            // Gera os outputs
            var outputs = _selectedRecipe.Outputs
                .Select(outputData => ItemInstanceFactory.Create(outputData))
                .ToList();

            return CraftingResult.Ok(outputs);
        }

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
    }
}
