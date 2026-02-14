using TMPro;
using UnityEngine;

namespace User_Interface
{
    public class ItemLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelText;

        /// <summary>
        /// Sets the text of the item label that is displayed above the item in the world.
        /// </summary>
        /// <param name="text"></param>
        public void SetLabelText(string text)
        {
            if (labelText != null)
            {
                labelText.text = text;
            }
            else
            {
                labelText = GetComponentInChildren<TextMeshProUGUI>();
                if (labelText != null)
                {
                    labelText.text = text;
                }
                else
                {
                    Debug.LogWarning($"TextMeshProUGUI 'labelText' is not assigned and not found in children on {gameObject.name}");
                }
            }
        }
    }
}
