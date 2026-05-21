using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pyrite.Grid
{
    public enum CellType
    {
        Empty,
        Structure,
        Conveyor,
        Machine,
        Wall,
        ResourceNode
    }

    [Serializable]
    public class GridCell
    {
        public Vector2Int coordinates;
        public CellType cellType;
        public GameObject occupant;

        public GridCell(Vector2Int coords, CellType type, GameObject obj)
        {
            coordinates = coords;
            cellType = type;
            occupant = obj;
        }
    }

    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }

        [Header("Grid Scale Settings")]
        [Tooltip("Width and length of each grid cell in world units.")]
        public float cellSize = 2.0f;

        [Tooltip("The default vertical baseline height for placing objects on the grid.")]
        public float gridYBaseline = 0.0f;

        private Dictionary<Vector2Int, GridCell> _gridCells = new Dictionary<Vector2Int, GridCell>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        /// <summary>
        /// Converts a 3D world position to 2D grid coordinates.
        /// </summary>
        public Vector2Int WorldToGrid(Vector3 worldPos)
        {
            int x = Mathf.RoundToInt(worldPos.x / cellSize);
            int z = Mathf.RoundToInt(worldPos.z / cellSize);
            return new Vector2Int(x, z);
        }

        /// <summary>
        /// Converts 2D grid coordinates back into a centered 3D world position.
        /// </summary>
        public Vector3 GridToWorld(Vector2Int gridPos, float heightOffset = 0f)
        {
            return new Vector3(gridPos.x * cellSize, gridYBaseline + heightOffset, gridPos.y * cellSize);
        }

        /// <summary>
        /// Returns true if the coordinate has an occupant.
        /// </summary>
        public bool IsCellOccupied(Vector2Int gridPos)
        {
            if (_gridCells.TryGetValue(gridPos, out var cell))
            {
                return cell.cellType != CellType.Empty;
            }
            return false;
        }

        /// <summary>
        /// Gets the type of occupancy at the specified coordinate.
        /// </summary>
        public CellType GetCellType(Vector2Int gridPos)
        {
            if (_gridCells.TryGetValue(gridPos, out var cell))
            {
                return cell.cellType;
            }
            return CellType.Empty;
        }

        /// <summary>
        /// Retrieves the occupying GameObject at the specified coordinate.
        /// </summary>
        public GameObject GetCellOccupant(Vector2Int gridPos)
        {
            if (_gridCells.TryGetValue(gridPos, out var cell))
            {
                return cell.occupant;
            }
            return null;
        }

        /// <summary>
        /// Registers occupancy of a cell by a structure. Returns false if already occupied.
        /// </summary>
        public bool SetCellOccupant(Vector2Int gridPos, GameObject occupant, CellType type)
        {
            if (type != CellType.Empty && IsCellOccupied(gridPos))
            {
                Debug.LogWarning($"[GridManager] Coordinate {gridPos} is already occupied by {_gridCells[gridPos].occupant?.name}!");
                return false;
            }

            if (type == CellType.Empty)
            {
                _gridCells.Remove(gridPos);
            }
            else
            {
                _gridCells[gridPos] = new GridCell(gridPos, type, occupant);
            }
            return true;
        }

        /// <summary>
        /// Unregisters occupancy, clearing the coordinate.
        /// </summary>
        public void ClearCell(Vector2Int gridPos)
        {
            if (_gridCells.ContainsKey(gridPos))
            {
                _gridCells.Remove(gridPos);
            }
        }

        /// <summary>
        /// Returns a snapshot dictionary of all currently active cell occupancies (useful for rendering Gizmos).
        /// </summary>
        public Dictionary<Vector2Int, GridCell> GetAllOccupiedCells()
        {
            return new Dictionary<Vector2Int, GridCell>(_gridCells);
        }
    }
}
