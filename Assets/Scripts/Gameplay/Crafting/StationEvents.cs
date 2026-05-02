using Assets.Scripts.Gameplay.Inventory;
using System;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Crafting
{
    /// <summary>
    /// Canal global de eventos de estacoes de crafting.
    /// Stations disparam aqui, UIs escutam aqui — sem referencias diretas entre eles.
    /// </summary>
    public static class StationEvents
    {
        public static event Action<MonoBehaviour, InventoryBase> OnStationOpened;
        public static event Action OnStationClosed;

        public static void NotifyOpened(MonoBehaviour station, InventoryBase playerInventory)
            => OnStationOpened?.Invoke(station, playerInventory);

        public static void NotifyClosed()
            => OnStationClosed?.Invoke();
    }
}
