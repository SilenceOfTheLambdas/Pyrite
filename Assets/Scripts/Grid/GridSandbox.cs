using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pyrite.Grid
{
    public class GridSandbox : MonoBehaviour
    {
        [Header("Prefabs for Testing")]
        [Tooltip("The visual block object that will be spawned upon clicking.")]
        public GameObject blockPrefab;

        [Tooltip("Optional visual hover preview object showing snap coordinate.")]
        public GameObject hoverPreviewInstance;

        [Header("Layer Config")]
        [Tooltip("The physical layer representing ground/terrain. Set to 'Default' or empty for fallback mathematical grid mapping.")]
        public LayerMask groundLayer;

        [Header("Scene Visualization")]
        [Tooltip("Number of cells drawn outward in all directions for visual editor grids.")]
        public int sandboxGridRadius = 15;

        private Vector2Int _currentHoveredCell;
        private bool _isHoveringValid = false;

        private void Start()
        {
            // If the user has not assigned a preview object, create a simple temporary placeholder cube if blockPrefab is present
            if (hoverPreviewInstance == null && blockPrefab != null)
            {
                hoverPreviewInstance = Instantiate(blockPrefab);
                hoverPreviewInstance.name = "Grid_HoverPreview";
                
                // Remove physics components from the preview instance
                foreach (var col in hoverPreviewInstance.GetComponentsInChildren<Collider>())
                {
                    Destroy(col);
                }
                foreach (var rb in hoverPreviewInstance.GetComponentsInChildren<Rigidbody>())
                {
                    Destroy(rb);
                }

                // Add a simple color shift or transparency indicator to the preview material if possible
                var renderer = hoverPreviewInstance.GetComponentInChildren<Renderer>();
                if (renderer != null && renderer.material != null)
                {
                    // Tint slightly transparent blue
                    renderer.material.color = new Color(0f, 0.7f, 1f, 0.4f);
                }
            }
        }

        private void Update()
        {
            if (GridManager.Instance == null) return;

            UpdateMouseHover();
            HandlePlacementInput();
        }

        /// <summary>
        /// Raycasts from screen coordinates to map hovered pixels to physical grid points.
        /// </summary>
        private void UpdateMouseHover()
        {
            if (Camera.main == null || Mouse.current == null)
            {
                _isHoveringValid = false;
                if (hoverPreviewInstance != null) hoverPreviewInstance.SetActive(false);
                return;
            }

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            Vector3 hitPoint = Vector3.zero;
            bool hitGround = false;

            // Try standard physical raycast
            if (Physics.Raycast(ray, out RaycastHit hit, 150f, groundLayer))
            {
                hitPoint = hit.point;
                hitGround = true;
            }
            else
            {
                // Fallback: Mathematical plane intersection at baseline height Y = GridManager.Instance.gridYBaseline
                float yBaseline = GridManager.Instance.gridYBaseline;
                Plane baselinePlane = new Plane(Vector3.up, new Vector3(0, yBaseline, 0));
                
                if (baselinePlane.Raycast(ray, out float rayDistance))
                {
                    hitPoint = ray.GetPoint(rayDistance);
                    hitGround = true;
                }
            }

            if (hitGround)
            {
                _currentHoveredCell = GridManager.Instance.WorldToGrid(hitPoint);
                _isHoveringValid = true;

                // Move preview instance
                if (hoverPreviewInstance != null)
                {
                    hoverPreviewInstance.SetActive(true);
                    hoverPreviewInstance.transform.position = GridManager.Instance.GridToWorld(_currentHoveredCell);
                }
            }
            else
            {
                _isHoveringValid = false;
                if (hoverPreviewInstance != null) hoverPreviewInstance.SetActive(false);
            }
        }

        /// <summary>
        /// Listens for mouse clicks to place (Left Click) or remove (Right Click) grid structures.
        /// </summary>
        private void HandlePlacementInput()
        {
            if (!_isHoveringValid || Mouse.current == null) return;

            // 1. PLACE BLOCK (Left Click)
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (!GridManager.Instance.IsCellOccupied(_currentHoveredCell))
                {
                    Vector3 spawnPos = GridManager.Instance.GridToWorld(_currentHoveredCell);
                    
                    if (blockPrefab != null)
                    {
                        GameObject placedBlock = Instantiate(blockPrefab, spawnPos, Quaternion.identity);
                        placedBlock.name = $"PlacedBlock_{_currentHoveredCell.x}_{_currentHoveredCell.y}";
                        
                        // Register occupancy in GridManager
                        GridManager.Instance.SetCellOccupant(_currentHoveredCell, placedBlock, CellType.Structure);
                        Debug.Log($"[GridSandbox] Placed structure at cell {_currentHoveredCell}");
                    }
                    else
                    {
                        // Spawn a fallback visual cube
                        GameObject placedCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        placedCube.transform.position = spawnPos;
                        placedCube.transform.localScale = Vector3.one * (GridManager.Instance.cellSize * 0.95f);
                        placedCube.name = $"PlacedCube_{_currentHoveredCell.x}_{_currentHoveredCell.y}";

                        GridManager.Instance.SetCellOccupant(_currentHoveredCell, placedCube, CellType.Structure);
                        Debug.Log($"[GridSandbox] Placed fallback cube at cell {_currentHoveredCell}");
                    }
                }
                else
                {
                    Debug.LogWarning($"[GridSandbox] Cannot place! Cell {_currentHoveredCell} is already occupied.");
                }
            }

            // 2. REMOVE BLOCK (Right Click)
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (GridManager.Instance.IsCellOccupied(_currentHoveredCell))
                {
                    GameObject occupant = GridManager.Instance.GetCellOccupant(_currentHoveredCell);
                    if (occupant != null)
                    {
                        Destroy(occupant);
                    }
                    
                    GridManager.Instance.ClearCell(_currentHoveredCell);
                    Debug.Log($"[GridSandbox] Removed structure at cell {_currentHoveredCell}");
                }
            }
        }

        /// <summary>
        /// Renders visual grid outlines and occupancy indicators in Unity's Scene view and in play-mode Gizmos.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (GridManager.Instance == null) return;

            float cellSize = GridManager.Instance.cellSize;
            float y = GridManager.Instance.gridYBaseline;

            // Draw horizontal and vertical grid bounds
            Gizmos.color = new Color(1f, 1f, 1f, 0.2f); // Light semi-transparent white lines
            for (int i = -sandboxGridRadius; i <= sandboxGridRadius; i++)
            {
                // Parallel to Z axis
                Vector3 startZ = new Vector3(i * cellSize, y, -sandboxGridRadius * cellSize);
                Vector3 endZ = new Vector3(i * cellSize, y, sandboxGridRadius * cellSize);
                Gizmos.DrawLine(startZ, endZ);

                // Parallel to X axis
                Vector3 startX = new Vector3(-sandboxGridRadius * cellSize, y, i * cellSize);
                Vector3 endX = new Vector3(sandboxGridRadius * cellSize, y, i * cellSize);
                Gizmos.DrawLine(startX, endX);
            }

            // Draw visual boxes showing active occupied cells in grid space
            Dictionary<Vector2Int, GridCell> occupiedCells = GridManager.Instance.GetAllOccupiedCells();
            foreach (var kvp in occupiedCells)
            {
                if (kvp.Value.cellType != CellType.Empty)
                {
                    // Draw a semi-transparent red cube over occupied cells
                    Vector3 cellCenter = GridManager.Instance.GridToWorld(kvp.Key, 0.05f);
                    Gizmos.color = new Color(1f, 0f, 0f, 0.4f);
                    Gizmos.DrawCube(cellCenter, new Vector3(cellSize * 0.95f, 0.1f, cellSize * 0.95f));
                }
            }

            // Highlight the active mouse-hovered coordinate cell if valid
            if (_isHoveringValid)
            {
                Vector3 cellCenter = GridManager.Instance.GridToWorld(_currentHoveredCell, 0.06f);
                Gizmos.color = new Color(0f, 0.7f, 1f, 0.5f); // Glowing sky blue
                Gizmos.DrawCube(cellCenter, new Vector3(cellSize * 0.98f, 0.12f, cellSize * 0.98f));
            }
        }
    }
}
