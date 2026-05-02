using Assets.Scripts.Gameplay.Crafting;
using Assets.Scripts.UI.Stations;
using UnityEngine;

public class RecipeListUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject recipeItemPrefab;

    private CraftingRecipe[] _recipes;

    public void Bind(CraftingRecipe[] availableRecipes, ForgeStationUI forgeStationUI)
    {
        Unbind();

        _recipes = availableRecipes ?? new CraftingRecipe[0];

        BuildRecipes(forgeStationUI);
    }

    private void BuildRecipes(ForgeStationUI forgeStationUI)
    {
        for (int i = 0; i < _recipes.Length; i++)
        {
            var gameObject = Instantiate(recipeItemPrefab, container);
            var ui = gameObject.GetComponent<RecipeItemUI>();
            ui.Bind(_recipes[i], forgeStationUI);
        }
    }

    public void Unbind()
    {
        _recipes = null;
        ClearRecipes();
    }

    private void ClearRecipes()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }
}
