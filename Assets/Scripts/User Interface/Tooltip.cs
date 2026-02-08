using TMPro;
using UnityEngine;

namespace User_Interface
{
    public class Tooltip : MonoBehaviour
    {
        [Header("Item Requirements Text")] public TextMeshProUGUI itemLevelRequirementText;
        public TextMeshProUGUI itemStrengthRequirementText;
        public TextMeshProUGUI itemDexterityRequirementText;
        public TextMeshProUGUI itemIntelligenceRequirementText;

        private void OnValidate()
        {
            if (itemLevelRequirementText == null)
                Debug.LogError("Tooltip item level requirement text is null!");
            if (itemStrengthRequirementText == null)
                Debug.LogError("Tooltip item strength requirement text is null!");
            if (itemDexterityRequirementText == null)
                Debug.LogError("Tooltip item dexterity requirement text is null!");
            if (itemIntelligenceRequirementText == null)
                Debug.LogError("Tooltip item intelligence requirement text is null!");
        }
    }
}