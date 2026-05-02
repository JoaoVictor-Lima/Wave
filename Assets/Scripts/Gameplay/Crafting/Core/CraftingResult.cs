using Assets.ScriptableObjects;
using System.Collections.Generic;

namespace Assets.Scripts.Gameplay.Crafting
{
    public class CraftingResult
    {
        public bool Success;
        public string FailReason;
        public List<ItemInstance> Outputs;

        public static CraftingResult Fail(string reason) =>
            new CraftingResult { Success = false, FailReason = reason };

        public static CraftingResult Ok(List<ItemInstance> outputs) =>
            new CraftingResult { Success = true, Outputs = outputs };
    }
}
