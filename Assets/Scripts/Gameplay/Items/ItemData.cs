using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    public ItemData()
    {
        Modules = new List<ItemModule>();
    }

    [Header("Base Info")]
    public string ItemName;
    public ItemType ItemType;

    [TextArea]
    public string Description;

    [Header("Visual")]
    public Sprite Icon;

    [Header("Inventory Config")]
    public bool Stackable = true;
    public int MaxStack = 1;
    public float Weight = 0.1f;

    [Header("Modules")]
    [SerializeReference]
    [SubclassSelector]
    public List<ItemModule> Modules;

    public bool HasModule<T>() where T : ItemModule
    {
        return Modules.OfType<T>().Any();
    }

    public T GetModule<T>() where T : ItemModule
    {
        return Modules.OfType<T>().FirstOrDefault();
    }

    public IEnumerable<T> GetModules<T>() where T : ItemModule
    {
        return Modules.OfType<T>();
    }
}