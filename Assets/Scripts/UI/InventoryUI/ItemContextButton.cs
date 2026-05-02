using Assets.Scripts.Gameplay.Items.Types;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public class ItemContextButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private Button button;

        public void Setup(ItemOptionType option, Action callback)
        {
            label.text = option.ToString();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => callback());
        }
    }
}
