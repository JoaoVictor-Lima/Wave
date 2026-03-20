using Assets.Scripts.Gameplay.Items.Weapon;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Gameplay.Items.Modules
{
    [Serializable]
    public class WeaponModule : ItemModule
    {
        public WeaponArchetype Archetype;
        public HandType HandType;

        public List<WeaponPartRequirement> RequiredParts;
    }
}
