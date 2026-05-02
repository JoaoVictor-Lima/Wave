using Assets.Scripts.Gameplay.Items.Weapon;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Crafting
{
    /// <summary>
    /// Define uma configuracao de montagem de arma: qual ItemData e produzido
    /// e quais WeaponPartTypes sao necessarios nos slots.
    /// Crie um SO por tipo de arma (ex: ShortSword Config, LongSword Config).
    /// </summary>
    [CreateAssetMenu(menuName = "Crafting/Weapon Assembly Config")]
    public class WeaponAssemblyConfig : ScriptableObject
    {
        public string ConfigName;
        public ItemData WeaponData;
        public List<WeaponPartType> RequiredPartTypes = new();
    }
}
