using Assets.ScriptableObjects;
using Assets.Scripts.Gameplay.Crafting;
using Assets.Scripts.Gameplay.Crafting.Stations;
using Assets.Scripts.Gameplay.Inventory;
using UnityEngine;

namespace Assets.Scripts.UI.Stations
{
    /// <summary>
    /// UI da fornalha. So lida com visual — toda logica esta no ForgeStation.
    /// Recebe o ForgeStation e o inventario do player ao abrir.
    /// </summary>
    public class ForgeStationUI : MonoBehaviour
    {
        [SerializeField] private InventoryGridUI playerInventoryGrid;
        [SerializeField] private InventoryGridUI outputInventoryGrid;
        [SerializeField] private RecipeListUI recipeList;
        [SerializeField] private GameObject inputSlotPrefab;
        [SerializeField] private GameObject recipeSelected;
        [SerializeField] private Transform inputContainer;

        private InventorySlotUI[] _inputSlots;
        private InventoryBase inventory;

        private ForgeStation _station;

        private void Awake()
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);

            inventory = new InventoryBase();
            inventory.SetSlots(5);
        }

        public void Open(ForgeStation station, InventoryBase playerInventory)
        {
            _station = station;

            playerInventoryGrid.Bind(playerInventory);
            recipeList.Bind(station.AvailableRecipes, this);

            _station.OnProgressChanged += OnProgressChanged;
            _station.OnProcessingStarted += OnProcessingStarted;
            _station.OnProcessingCompleted += OnProcessingCompleted;
            _station.OnItemProduced += OnItemProduced;

            BuildOutputSlots();

            gameObject.SetActive(true);
        }

        public void Close()
        {
            if (_station != null)
            {
                _station.OnProgressChanged -= OnProgressChanged;
                _station.OnProcessingStarted -= OnProcessingStarted;
                _station.OnProcessingCompleted -= OnProcessingCompleted;
                _station.OnItemProduced -= OnItemProduced;
            }

            playerInventoryGrid.Unbind();
            _station = null;

            gameObject.SetActive(false);
        }

        private void OnProgressChanged(float progress)
        {
            // TODO: atualizar barra de progresso
        }

        private void OnProcessingStarted()
        {
            // TODO: feedback visual de inicio
        }

        private void OnProcessingCompleted()
        {
            // TODO: feedback visual de conclusao
        }

        private void OnItemProduced(ItemInstance item)
        {
            // TODO: animacao do item aparecendo no output
        }

        public void SetRecipe(CraftingRecipe recipe)
        {
            ClearInputContainer();
            BuildInputSlots(recipe.Inputs.Count);

            var uiRecipeSelected = recipeSelected.GetComponent<InputSlot>();
            uiRecipeSelected.Bind(recipe);
        }

        private void BuildInputSlots(int numberOfSlots)
        {
            _inputSlots = new InventorySlotUI[numberOfSlots];

            for (int i = 0; i < numberOfSlots; i++)
            {
                var go = Instantiate(inputSlotPrefab, inputContainer);
                var ui = go.GetComponent<InventorySlotUI>();
                ui.Bind(i);
                _inputSlots[i] = ui;
            }
        }

        private void ClearInputContainer()
        {
            foreach (Transform child in inputContainer)
            {
                Destroy(child.gameObject);
            }
        }

        private void BuildOutputSlots()
        {
            outputInventoryGrid.HardClearSlots();
            outputInventoryGrid.Bind(inventory);
        }
    }
}
