using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace User_Interface
{
    public class VirtualCursorUiInput : MonoBehaviour
    {
        public static VirtualCursorUiInput Instance;
        
        [SerializeField] private CursorManager cursorManager;

        private readonly List<RaycastResult> _raycastResults = new();
        private PointerEventData _pointerEventData;
        private GameObject _currentHoveredObject;
        private GameObject _pressedObject;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }

            Instance = this;
            
            _pointerEventData = new PointerEventData(EventSystem.current);
        }

        private void Update()
        {
            if (cursorManager == null || EventSystem.current == null)
                return;

            GameObject hoveredObject = RaycastUI();

            HandleHover(hoveredObject);
            HandleClick(hoveredObject);
        }

        private GameObject RaycastUI()
        {
            _raycastResults.Clear();

            _pointerEventData.Reset();
            _pointerEventData.position = cursorManager.VirtualPosition;

            EventSystem.current.RaycastAll(_pointerEventData, _raycastResults);

            if (_raycastResults.Count == 0)
                return null;

            return _raycastResults[0].gameObject;
        }

        private void HandleHover(GameObject hoveredObject)
        {
            if (_currentHoveredObject == hoveredObject)
                return;

            if (_currentHoveredObject != null)
            {
                ExecuteEvents.Execute(
                    _currentHoveredObject,
                    _pointerEventData,
                    ExecuteEvents.pointerExitHandler);
            }

            _currentHoveredObject = hoveredObject;

            if (_currentHoveredObject != null)
            {
                ExecuteEvents.Execute(
                    _currentHoveredObject,
                    _pointerEventData,
                    ExecuteEvents.pointerEnterHandler);
            }
        }

        private void HandleClick(GameObject hoveredObject)
        {
            if (Mouse.current == null)
                return;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                _pressedObject = hoveredObject;

                if (_pressedObject != null)
                {
                    ExecuteEvents.Execute(
                        _pressedObject,
                        _pointerEventData,
                        ExecuteEvents.pointerDownHandler);
                }
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                if (_pressedObject != null)
                {
                    ExecuteEvents.Execute(
                        _pressedObject,
                        _pointerEventData,
                        ExecuteEvents.pointerUpHandler);

                    if (_pressedObject == hoveredObject)
                    {
                        ExecuteEvents.Execute(
                            _pressedObject,
                            _pointerEventData,
                            ExecuteEvents.pointerClickHandler);

                        ExecuteEvents.Execute(
                            _pressedObject,
                            _pointerEventData,
                            ExecuteEvents.submitHandler);
                    }
                }

                _pressedObject = null;
            }
        }

        public bool IsPointerOverUI()
        {
            return RaycastUI() != null;
        }
    }
}
