using Assets.ScriptableObjects;
using Assets.Scripts.Gameplay.Items;
using Assets.Scripts.Gameplay.Loot;
using System.Collections.Generic;

public class LootCalculator
{
    // TODO: If item type is weapon, generate weapon parts first.
    public static List<ItemInstance> GenerateLoot(LootEntry[] entries)
    {
        List<ItemInstance> itemsInstanceResult = new List<ItemInstance>();

        foreach (var entry in entries)
        {
            if (UnityEngine.Random.value > entry.dropChance)
                continue;

            int amount = UnityEngine.Random.Range(entry.minAmount, entry.maxAmount + 1);

            for (int i = 0; i < amount; i++)
            {
                var itemInstance = ItemInstanceFactory.Create(entry.item, 1);
                itemsInstanceResult.Add(itemInstance);
            }
        }

        return itemsInstanceResult;
    }
}
