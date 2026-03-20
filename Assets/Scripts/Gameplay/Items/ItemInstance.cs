using Assets.Scripts.Gameplay.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.ScriptableObjects
{
    [Serializable]
    public class ItemInstance
    {
        public ItemInstance(ItemData itemData, int quantity)
        {
            Id = Guid.NewGuid();
            ItemData = itemData;
            Quantity = quantity;
            InstancedModules = new List<InstancedModule>();
        }

        public ItemInstance(ItemData itemData, int quantity, List<InstancedModule> instancedModules)
            : this(itemData, quantity)
        {
            InstancedModules = instancedModules;
        }

        public Guid Id;
        public ItemData ItemData;
        public int Quantity;

        [SerializeReference]
        [SubclassSelector]
        public List<InstancedModule> InstancedModules;

        public T GetInstanceModule<T>() where T : InstancedModule
        {
            return InstancedModules.Find(m => m is T) as T;
        }
    }
}
