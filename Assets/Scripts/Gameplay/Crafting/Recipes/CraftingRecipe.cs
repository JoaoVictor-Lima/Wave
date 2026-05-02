using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Crafting
{
    [CreateAssetMenu(menuName = "Crafting/Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        public Sprite Icon;
        public string RecipeName;

        [SerializeField]
        public List<CraftingIngredient> Inputs = new();

        [SerializeField]
        public List<ItemData> Outputs = new();

        public float CraftTime;
    }
}
