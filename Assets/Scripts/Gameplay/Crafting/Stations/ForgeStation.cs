using Assets.ScriptableObjects;
using Assets.Scripts.Entities.Player;
using Assets.Scripts.Gameplay.Interact;
using Assets.Scripts.Gameplay.Inventory;
using Assets.Scripts.Gameplay.Items;
using System;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Crafting.Stations
{
    /// <summary>
    /// Fornalha: converte minerios em lingotes usando combustivel e tempo.
    ///
    /// Fluxo:
    ///   1. Player coloca minerio no InputSlot e combustivel no FuelSlot
    ///   2. Player chama StartSmelting() via UI
    ///   3. Timer roda em Update(), progresso e disparado via OnProgressChanged
    ///   4. Ao completar, item vai para OutputInventory e OnItemProduced e disparado
    ///   5. Player arrasta do OutputInventory para o proprio inventario
    /// </summary>
    [AddComponentMenu("Crafting/Forge Station")]
    public class ForgeStation : MonoBehaviour, IInteractable
    {
        [SerializeField] private CraftingRecipe[] availableRecipes;
        [SerializeField] private int outputSlots = 4;

        // Slots fixos - nao mudam com receita
        public CraftingSlot InputSlot { get; private set; }
        public CraftingSlot FuelSlot { get; private set; }

        // Inventario de saida - itens ficam aqui ate o player coletar
        public InventoryBase OutputInventory { get; private set; }

        // Estado de processamento
        public bool IsProcessing { get; private set; }
        public float Progress { get; private set; }

        private CraftingRecipe _selectedRecipe;
        private float _timer;

        // Eventos para a UI
        public event Action<float> OnProgressChanged;   // 0-1
        public event Action<ItemInstance> OnItemProduced;
        public event Action OnProcessingStarted;
        public event Action OnProcessingCompleted;
        public CraftingRecipe[] AvailableRecipes => availableRecipes;
        public CraftingRecipe SelectedRecipe => _selectedRecipe;

        private void Awake()
        {
            InputSlot = new CraftingSlot { Label = "Minerio" };
            FuelSlot = new CraftingSlot { Label = "Combustivel" };
            OutputInventory = new InventoryBase(outputSlots);
        }

        private void Update()
        {
            if (!IsProcessing) return;

            _timer += Time.deltaTime;
            Progress = Mathf.Clamp01(_timer / _selectedRecipe.CraftTime);
            OnProgressChanged?.Invoke(Progress);

            if (_timer >= _selectedRecipe.CraftTime)
                CompleteProcessing();
        }

        public void SelectRecipe(CraftingRecipe recipe)
        {
            if (IsProcessing)
            {
                Debug.LogWarning("Nao e possivel trocar receita durante o processamento.");
                return;
            }

            _selectedRecipe = recipe;
        }

        public bool CanSmelt()
        {
            if (_selectedRecipe == null) return false;
            if (IsProcessing) return false;
            if (InputSlot.IsEmpty || FuelSlot.IsEmpty) return false;

            // Verifica se o input bate com a receita
            foreach (var ingredient in _selectedRecipe.Inputs)
            {
                if (InputSlot.CurrentItem?.ItemData != ingredient.ItemData) return false;
                if (InputSlot.CurrentItem.Quantity < ingredient.Quantity) return false;
            }

            return true;
        }

        public void StartSmelting()
        {
            if (!CanSmelt()) return;

            // Consome os itens dos slots
            InputSlot.Clear();
            FuelSlot.Clear();

            IsProcessing = true;
            _timer = 0f;
            Progress = 0f;

            OnProcessingStarted?.Invoke();
        }

        private void CompleteProcessing()
        {
            IsProcessing = false;
            _timer = 0f;
            Progress = 0f;

            foreach (var outputData in _selectedRecipe.Outputs)
            {
                var item = ItemInstanceFactory.Create(outputData);
                OutputInventory.AddItem(item);
                OnItemProduced?.Invoke(item);
            }

            OnProcessingCompleted?.Invoke();
        }

        public void Interact(InteractionContext context)
        {
            var playerInventory = context.Source.GetComponent<PlayerInventory>();

            if (playerInventory == null)
            {
                Debug.LogWarning("ForgeStation: quem interagiu nao tem PlayerInventory.");
                return;
            }

            StationEvents.NotifyOpened(this, playerInventory.Inventory);
        }
    }
}
