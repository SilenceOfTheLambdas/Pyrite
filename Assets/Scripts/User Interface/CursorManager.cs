using UnityEngine;

namespace User_Interface
{
    public class CursorManager : MonoBehaviour
    {
        public static CursorManager Instance;
        
        [Header("Cursor Controller")]
        [SerializeField] private UICursor uiCursor;
        
        [Header("Cursor Sprites")]
        [SerializeField] private Sprite defaultCursor;
        [SerializeField] private Sprite interactCursor;
        [SerializeField] private Sprite attackCursor;
        
        private CursorType _currentCursorType = CursorType.None;

        public Vector2 VirtualPosition => uiCursor.VirtualPosition;
        
        public enum CursorType
        {
            None,
            Default,
            Interact,
            Attack
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            SetCursor(CursorType.Default);
        }

        public void SetCursor(CursorType cursorType)
        {
            if (_currentCursorType == cursorType) return;
            _currentCursorType = cursorType;

            Sprite cursorSprite = GetCursorSprite(cursorType);

            if (cursorSprite == null)
            {
                Debug.LogWarning($"Cursor sprite for cursor type {cursorType} is null.");
                return;
            }
            
            uiCursor.SetCursorSprite(cursorSprite);
        }
        
        public void ResetCursor()
        {
            SetCursor(CursorType.Default);
        }

        public void ShowCursor()
        {
            Cursor.visible = false;
            uiCursor.ShowCursor();
        }

        public void HideCursor()
        {
            uiCursor.HideCursor();
        }

        public void FreezeCursor()
        {
            uiCursor.FreezeCursor();
        }

        public void UnfreezeCursor()
        {
            uiCursor.UnfreezeCursor();
        }

        private Sprite GetCursorSprite(CursorType cursorType)
        {
            return cursorType switch
            {
                CursorType.Default => defaultCursor,
                CursorType.Attack => attackCursor,
                CursorType.Interact => interactCursor,
                _ => defaultCursor
            };
        }
    }
}
