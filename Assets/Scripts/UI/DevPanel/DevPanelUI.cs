using Assets.Scripts.Entities.Player;
using Assets.Scripts.Gameplay.Items;
using System.Collections.Generic;
using UnityEngine;

public class DevPanelUI : MonoBehaviour
{
    [Header("Dev Spawn")]
    public ItemData ShortSwordData;
    public List<ItemData> ComponentItemsForTest;
    public Transform SpawnPoint;

    public PlayerInventory PlayerInventory;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void CreateSword()
    {
        var instanceItem = ItemInstanceFactory.Create(ShortSwordData, 1, ComponentItemsForTest);
        Debug.Log("Created item instance: " + instanceItem.Id);

        PlayerInventory.Inventory.AddItem(instanceItem);

        //var assembly = new WeaponAssembly();

        //assembly.AssembleWeapon(instanceItem, SpawnPoint);
    }
}