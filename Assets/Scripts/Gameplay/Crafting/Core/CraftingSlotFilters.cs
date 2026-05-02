using Assets.ScriptableObjects;
using Assets.Scripts.Gameplay.Items.Modules;
using Assets.Scripts.Gameplay.Items.Weapon;
using System;

namespace Assets.Scripts.Gameplay.Crafting
{
    /// <summary>Aceita apenas um ItemData especifico. Usado para receitas de Forja e Bigorna.</summary>
    [Serializable]
    public class ItemDataFilter : CraftingSlotFilter
    {
        public ItemData Required;

        public override bool Accepts(ItemInstance item) =>
            item?.ItemData == Required;
    }

    /// <summary>Aceita qualquer item de um ItemType especifico. Ex: Resource.</summary>
    [Serializable]
    public class ItemTypeFilter : CraftingSlotFilter
    {
        public ItemType Required;

        public override bool Accepts(ItemInstance item) =>
            item?.ItemData.ItemType == Required;
    }

    /// <summary>Aceita qualquer parte de arma do tipo especificado. Usado na mesa de montagem.</summary>
    [Serializable]
    public class WeaponPartTypeFilter : CraftingSlotFilter
    {
        public WeaponPartType PartType;

        public override bool Accepts(ItemInstance item)
        {
            var module = item?.ItemData.GetModule<WeaponPartShapeModule>();
            return module != null && module.PartType == PartType;
        }
    }
}
