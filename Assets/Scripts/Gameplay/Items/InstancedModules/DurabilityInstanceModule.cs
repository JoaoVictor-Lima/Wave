using System;

namespace Assets.Scripts.Gameplay.Items.InstancedModules
{
    [Serializable]
    public class DurabilityInstanceModule : InstancedModule
    {
        public float MaxDurability;
        public float CurrentDurability;
    }
}
