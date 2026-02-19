using System;
using TMPro;
using UnityEngine;

namespace User_Interface
{
    public class ItemLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelText;

        private void Start()
        {
            labelText = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            ItemLabelManager.Instance.RegisterLabel(this);
        }

        private void OnDisable()
        {
            ItemLabelManager.Instance.UnRegisterLabel(this);
        }

        /// <summary>
        /// Sets the text of the item label that is displayed above the item in the world. And also adjusts the colour
        /// of the text based on the rarity colour parameter (item items rarity).
        /// </summary>
        /// <param name="text"></param>
        /// <param name="rarityColour"></param>
        public void SetLabelTextAndRarity(string text, Color rarityColour)
        {
            if (labelText == null) return;
            
            labelText.text = text;
            labelText.color = rarityColour;
        }

        private void OnDestroy()
        {
            ItemLabelManager.Instance.UnRegisterLabel(this);
        }

        [Serializable]
        public struct LabelRarityColour
        {
            public static Color CommonColour = Color.white;
            public static Color UncommonColour = new(68, 141, 203);
            public static Color RareColour = new(255, 238, 30);
            public static Color EpicColour = new(220, 0, 255);
            public static Color UniqueColour = new(68, 141, 203);
            
            public static Color GetColourForRarity(RpgManager.ItemRarity rarity)
            {
                return rarity switch
                {
                    RpgManager.ItemRarity.Common => CommonColour,
                    RpgManager.ItemRarity.Uncommon => UncommonColour,
                    RpgManager.ItemRarity.Rare => RareColour,
                    RpgManager.ItemRarity.Epic => EpicColour,
                    RpgManager.ItemRarity.Unique => UniqueColour,
                    _ => CommonColour
                };
            }
        }
    }
}
