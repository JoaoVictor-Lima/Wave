using Assets.Scripts.Gameplay.Items.Types;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Gameplay.Items.Modules
{
    [Serializable]
    public class ItemOptionsModule : ItemModule
    {
        //TODO: Tranform in arrey for better performance and less memory usage
        public List<ItemOptionType> Options;
    }
}
