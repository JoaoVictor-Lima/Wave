using System;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Crafting
{
    [Serializable]
    public class CraftingIngredient
    {
        public ItemData ItemData;

        [Min(1)]
        public int Quantity = 1;
    }
}
