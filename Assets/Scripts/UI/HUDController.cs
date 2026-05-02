using Assets.Scripts.Characters.Player;
using Assets.Scripts.Entities.Player;
using Assets.Scripts.Gameplay.Crafting;
using Assets.Scripts.Gameplay.Crafting.Stations;
using Assets.Scripts.Gameplay.Inventory;
using Assets.Scripts.UI.Stations;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Ponto central da UI do jogador local.
    /// Em multiplayer, cada cliente inicializa este HUD apenas com o seu proprio player.
    /// Componentes de gameplay nao referenciam UI diretamente.
    /// Stations disparam StationEvents — HUDController decide qual UI abrir.
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private PlayerHealthUI healthUI;
        [SerializeField] private PlayerInventoryUI inventoryUI;
        [SerializeField] private EquipmentUI equipmentUI;
        [SerializeField] private ForgeStationUI furnaceUI;

        public void Initialize(GameObject localPlayer)
        {
            var health = localPlayer.GetComponent<Health>();
            var playerInventory = localPlayer.GetComponent<PlayerInventory>();
            var playerEquipment = localPlayer.GetComponent<PlayerEquipment>();
            var playerController = localPlayer.GetComponent<PlayerController>();

            healthUI.Initialize(health);
            equipmentUI.Initialize(playerEquipment.Equipment);
            inventoryUI.Initialize(playerInventory.Inventory, playerEquipment);

            playerInventory.OnOpenRequested += inventoryUI.Open;
            playerInventory.OnCloseRequested += inventoryUI.Close;

            playerController.OnReturn += CloseAll;

            StationEvents.OnStationOpened += OnStationOpened;
        }

        private void OnDestroy()
        {
            StationEvents.OnStationOpened -= OnStationOpened;
        }

        private void OnStationOpened(MonoBehaviour station, InventoryBase playerInventory)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            switch (station)
            {
                case ForgeStation forge:
                    furnaceUI.Open(forge, playerInventory);
                    break;

                    // Adicionar novos casos aqui conforme novas stations forem criadas:
                    // case AnvilStation anvil:
                    //     anvilUI.Open(anvil, playerInventory);
                    //     break;
            }
        }

        private void CloseAll()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            inventoryUI.Close();
            furnaceUI.Close();
        }
    }
}
