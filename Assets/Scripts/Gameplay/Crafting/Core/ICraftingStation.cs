using System.Collections.Generic;

namespace Assets.Scripts.Gameplay.Crafting
{
    public interface ICraftingStation
    {
        IReadOnlyList<CraftingSlot> Slots { get; }
        bool CanCraft();
        CraftingResult Craft();
    }
}
