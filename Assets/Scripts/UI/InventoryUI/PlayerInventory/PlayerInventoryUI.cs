using Assets.Scripts.Characters.Player;
using Assets.Scripts.Gameplay.Inventory;
using Assets.Scripts.UI.Inventory;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// UI do inventario do player local.
    /// Sempre exibe o mesmo inventario — nunca troca para externo.
    /// Usa InventoryGridUI internamente para popular os slots.
    /// </summary>
    public class PlayerInventoryUI : MonoBehaviour
    {
        [SerializeField] private InventoryGridUI grid;
        [SerializeField] private ItemContextMenu contextMenu;

        private void Awake()
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }

        public void Initialize(InventoryBase inventory, PlayerEquipment equipment)
        {
            contextMenu.Initialize(equipment);
            grid.Bind(inventory, contextMenu);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            contextMenu.Close();
            gameObject.SetActive(false);
        }
    }
}
