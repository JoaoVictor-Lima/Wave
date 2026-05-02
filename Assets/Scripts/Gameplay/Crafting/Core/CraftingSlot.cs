using Assets.ScriptableObjects;
using System;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Crafting
{
    [Serializable]
    public class CraftingSlot
    {
        public string Label;
        public bool IsOptional;

        [SerializeReference]
        [SubclassSelector]
        public CraftingSlotFilter Filter;

        public ItemInstance CurrentItem { get; private set; }

        public bool TryPlace(ItemInstance item)
        {
            if (Filter != null && !Filter.Accepts(item))
                return false;

            CurrentItem = item;
            return true;
        }

        /// <summary>Remove o item do slot e o retorna para ser devolvido ao inventario.</summary>
        public ItemInstance Clear()
        {
            var item = CurrentItem;
            CurrentItem = null;
            return item;
        }

        public bool IsEmpty => CurrentItem == null;
        public bool IsFilled => CurrentItem != null;
    }
}
