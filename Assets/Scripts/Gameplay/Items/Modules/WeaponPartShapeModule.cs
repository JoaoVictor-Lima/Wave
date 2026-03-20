using Assets.Scripts.Gameplay.Items.Weapon;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Gameplay.Items.Modules
{
    [Serializable]
    public class WeaponPartShapeModule : ItemModule
    {
        public WeaponPartType PartType;

        public List<WeaponFamily> CompatibleFamilies;

        public float Length;
        public float DamageMultiplier;
    }
}
