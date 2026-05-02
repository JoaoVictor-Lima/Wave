using UnityEngine;

namespace Assets.Scripts.Gameplay.Crafting
{
    /// <summary>
    /// Mesa de crafting generica: suporta qualquer receita configurada no Inspector.
    /// Pode ser usada para handles, componentes de madeira, e outros itens mistos.
    /// No futuro pode ser especializada em subclasses (ex: WoodworkingTable).
    /// </summary>
    [AddComponentMenu("Crafting/Crafting Table Station")]
    public class CraftingTableStation : RecipeBasedStation { }
}
