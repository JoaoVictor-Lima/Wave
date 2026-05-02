using Assets.ScriptableObjects;
using System;

namespace Assets.Scripts.Gameplay.Crafting
{
    [Serializable]
    public abstract class CraftingSlotFilter
    {
        public abstract bool Accepts(ItemInstance item);
    }
}
