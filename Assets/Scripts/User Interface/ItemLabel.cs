using System;
using System.Collections;
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
            StartCoroutine(SetLabelRoutine(text, rarityColour));
        }

        private IEnumerator SetLabelRoutine(string text, Color rarityColour)
        {
            yield return new WaitForEndOfFrame();
            if (labelText == null) labelText = GetComponentInChildren<TextMeshProUGUI>();
            if (labelText != null)
            {
                labelText.text = text;
                labelText.color = rarityColour;
                labelText.ForceMeshUpdate();
            }
        }

        private void OnDestroy()
        {
            ItemLabelManager.Instance.UnRegisterLabel(this);
        }

        [Serializable]
        public struct LabelRarityColour
        {
            public static Color CommonColour = Color.white;
            public static Color UncommonColour = new Color32(68, 141, 203, 255);
            public static Color RareColour = new Color32(255, 238, 30, 255);
            public static Color EpicColour = new Color32(220, 0, 255, 255);
            public static Color UniqueColour = new Color32(203, 77, 0, 255);
            
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
