using UnityEngine;

namespace Assets.Scripts.Gameplay.Loot
{
    [CreateAssetMenu(menuName = "Loot/LootTable")]
    public class LootTable : ScriptableObject
    {
        public LootEntry[] entries;
    }
}
