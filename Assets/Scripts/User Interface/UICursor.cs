using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace User_Interface
{
    public class UICursor : MonoBehaviour
    {
        public Vector2 VirtualPosition => _virtualPosition;
        
        [SerializeField] private RectTransform cursorTransform;
        [SerializeField] private Image cursorImage;

        private Vector2 _virtualPosition;
        private bool _isFrozen;
        private Vector2 _frozenPosition;
        private bool _ignoreNextDelta;

        private void Awake()
        {
            // Disable the default Unity hardware cursor
            Cursor.visible = false;

            
            _virtualPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
            cursorTransform.position = _virtualPosition;
            
        }

        private void Update()
        {
            if (Mouse.current == null)
                return;

            if (!_isFrozen)
            {
                if (_ignoreNextDelta)
                {
                    _ignoreNextDelta = false;
                }
                else
                {
                    Vector2 mouseDelta = Mouse.current.delta.ReadValue();
                    _virtualPosition += mouseDelta;
                    _virtualPosition = ClampToScreen(_virtualPosition);
                }
            }
            
            cursorTransform.position = _virtualPosition;
        }
        
        public void SetCursorSprite(Sprite sprite)
        {
            cursorImage.sprite = sprite;
        }

        public void FreezeCursor()
        {
            _isFrozen = true;
            _virtualPosition = cursorTransform.position;
            cursorTransform.position = _virtualPosition;
        }
        
        public void UnfreezeCursor()
        {
            _isFrozen = false;
            // Prevent one large delta from being applied immediately after camera rotation.
            _ignoreNextDelta = true;
        }

        public void ShowCursor() => cursorImage.enabled = true;
        public void HideCursor() => cursorImage.enabled = false;

        private static Vector2 ClampToScreen(Vector2 position)
        {
            position.x = Mathf.Clamp(position.x, 0f, Screen.width);
            position.y = Mathf.Clamp(position.y, 0f, Screen.height);
            return position;
        }
    }
}
