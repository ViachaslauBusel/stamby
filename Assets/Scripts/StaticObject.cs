using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObject : MonoBehaviour
{
    private Cell m_parentCell;

    private void Start()
    {
        m_parentCell = GetComponentInParent<Cell>();
    }
    private void OnDestroy()
    {
        m_parentCell.UpdateStatus();
    }
}
