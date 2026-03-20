using Assets.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public Inventory()
    {
        Items = new List<ItemInstance>();
    }

    public List<ItemInstance> Items { get; private set; }
    public event Action OnInventoryChanged;

    public void AddItem(ItemInstance item)
    {
        Items.Add(item);
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(ItemInstance item)
    {
        Items.Remove(item);
        OnInventoryChanged?.Invoke();
    }
    public void NotifyChanged()
    {
        Debug.Log("Inventory changed, notifying listeners...");
        OnInventoryChanged?.Invoke();
    }
}
