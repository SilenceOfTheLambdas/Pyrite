using System.Collections.Generic;
using UnityEngine;

namespace User_Interface
{
    public class ItemLabelManager : MonoBehaviour
    {
        public static ItemLabelManager Instance { get; private set; }

        private List<ItemLabel> _labels = new();
        private Camera _mainCamera;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            _mainCamera = Camera.main;
        }

        public void RegisterLabel(ItemLabel label) => _labels.Add(label);
        public void UnRegisterLabel(ItemLabel label) => _labels.Remove(label);

        private void LateUpdate()
        {
            if (_labels.Count <= 1) return;
            
            for (var i = 0; i < _labels.Count - 1; i++)
            {
                // Convert the world position of the label to screen space and update its position accordingly.
                var screenLabelRectA = LabelWorldRectToScreenRect(_labels[i].GetComponent<RectTransform>());
                var screenLabelRectB = LabelWorldRectToScreenRect(_labels[i+1].GetComponent<RectTransform>());
                if (screenLabelRectA.Overlaps(screenLabelRectB))
                {
                    // 1. Get the current screen position including depth of the second label.
                    Vector3 screenPosB = _mainCamera.WorldToScreenPoint(_labels[i+1].transform.position);
                    
                    // 2. Calculate the overlap height in pixels.
                    // To push B exactly above A: new Y = Top of A + half height of B (assuming pivot is centre).
                    float targetScreenY = screenLabelRectA.yMax + (screenLabelRectB.height * 0.5f);
                    
                    // 3. Update screen position.
                    screenPosB.y = targetScreenY;
                    
                    // 4. Convert back to world space using the original depth (z) of label B.
                    Vector3 newWorldPos = _mainCamera.ScreenToWorldPoint(screenPosB);
                    
                    // 5. Apply the new position
                    _labels[i+1].transform.position = newWorldPos;
                }
            }
        }

        private Rect LabelWorldRectToScreenRect(RectTransform labelRectTransform)
        {
            var worldCorners = new Vector3[4];
            labelRectTransform.GetWorldCorners(worldCorners);

            Vector2 minScreenPoint = _mainCamera.WorldToScreenPoint(worldCorners[0]);
            Vector2 maxScreenPoint = _mainCamera.WorldToScreenPoint(worldCorners[2]);

            return new Rect(minScreenPoint, maxScreenPoint - minScreenPoint);
        }
    }
}