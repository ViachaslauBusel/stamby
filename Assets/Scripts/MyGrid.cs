using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MyGrid : MonoBehaviour
{
    public static MyGrid Instance { get; private set; }
    /// <summary>Размер сетки(количество ячеек)</summary>
    [SerializeField] Vector2Int m_gridSize;
    /// <summary>Размер ячейки</summary>
    [SerializeField] Vector2 m_cellSize;
    /// <summary>Смещение строк</summary>
    [SerializeField] float m_offsetRow;

    private Cell[,] m_map;


    public Vector2Int GridSize => m_gridSize;
    public Vector2 CellSize => m_cellSize;
    public float OffsetRow => m_offsetRow;


    private void Awake()
    {
        Instance = this;
        m_map = new Cell[m_gridSize.y, m_gridSize.x];
        for (int row = 0; row < m_gridSize.y; row++)
        {
            for (int column = 0; column < m_gridSize.x; column++)
            {
                m_map[row, column] = transform.Find($"Cell_{row}_{column}").gameObject.GetComponent<Cell>();
                m_map[row, column].Init(row, column);
            }
        }
    }

    private void OnDrawGizmos()
    {
        for(int row =0; row <= m_gridSize.y; row++)
        {
            Vector3 p1 = transform.position;
            p1.x += m_offsetRow * row;
            p1.y += row * m_cellSize.y;
            Vector3 p2 = p1;
            p2.x += m_gridSize.x * m_cellSize.x;
            Gizmos.DrawLine(p1, p2);
        }

        for (int column = 0; column <= m_gridSize.x; column++)
        {
            Vector3 p1 = transform.position;
            p1.x += column * m_cellSize.x;
            Vector3 p2 = p1;
            p2.y += m_gridSize.y * m_cellSize.y;
            p2.x += m_offsetRow * m_gridSize.y;
            Gizmos.DrawLine(p1, p2);
        }
    }
    public Cell GetCell(Vector3 position)
    {
        position -= transform.position;

        Vector2Int cell = new Vector2Int();
        cell.y = Mathf.Clamp((int)(position.y / m_cellSize.y), 0, m_gridSize.y - 1);
        position.x -= cell.y * m_offsetRow;
        cell.x = Mathf.Clamp((int)(position.x / m_cellSize.x), 0, m_gridSize.x - 1);

        return m_map[cell.y, cell.x];
    }
    public Vector2 GetPosition(Cell cell)
    {
        return new Vector2(transform.position.x, transform.position.y)
          + new Vector2(cell.Column * m_cellSize.x + (cell.Row * m_offsetRow),
                        cell.Row * m_cellSize.y)
          + new Vector2(m_cellSize.x / 2.0f, m_cellSize.y / 2.0f);
    }

    public IEnumerable<Cell> GetAround(Cell cell)
    {
        for(int row = -1; row <= 1; row++)
        {
            for(int column = -1; column <= 1; column++)
            {
                if (row == 0 && column == 0) continue;
                if ((Mathf.Abs(row) + Mathf.Abs(column)) == 2) continue;
                int findRow = cell.Row + row;
                int findColumn = cell.Column + column;
                if (findRow < 0 || findRow >= m_map.GetLength(0)) continue;
                if (findColumn < 0 || findColumn >= m_map.GetLength(1)) continue;
                yield return m_map[findRow, findColumn];
            }
        }
    }

    public GameObject GetCellOBJ(Vector3 position)
    {
        position -= transform.position;

        Vector2Int cell = new Vector2Int(); 
        cell.y = Mathf.Clamp((int)(position.y / m_cellSize.y), 0, m_gridSize.y - 1);
        position.x -= cell.y * m_offsetRow;
        cell.x = Mathf.Clamp((int)(position.x / m_cellSize.x ), 0, m_gridSize.x - 1);
       
        return transform.Find($"Cell_{cell.y}_{cell.x}")?.gameObject;
    }
}
