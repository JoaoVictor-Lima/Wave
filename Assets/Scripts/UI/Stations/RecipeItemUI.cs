using Assets.Scripts.Gameplay.Crafting;
using Assets.Scripts.UI.Stations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeItemUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI name;

    private CraftingRecipe _craftingRecipe;
    private ForgeStationUI _forgeStationUI;

    public void Bind(CraftingRecipe craftingRecipe, ForgeStationUI forgeStationUI)
    {
        _craftingRecipe = craftingRecipe;
        _forgeStationUI = forgeStationUI;

        icon.sprite = craftingRecipe.Icon;
        name.text = craftingRecipe.RecipeName;
    }

    public void OnClick()
    {
        Debug.Log($"Clicked on recipe: {name.text}");
        _forgeStationUI.SetRecipe(_craftingRecipe);
    }
}
