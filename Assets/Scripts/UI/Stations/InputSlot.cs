using Assets.Scripts.Gameplay.Crafting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI name;

    public void Bind(CraftingRecipe recipe)
    {
        icon.sprite = recipe.Icon;
    }
}
