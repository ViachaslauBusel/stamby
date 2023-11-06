using GameField;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Walkers
{
    public class WalkerOrderRender : MonoBehaviour
    {
        private WalkerController m_controller;
        private SpriteRenderer m_spriteRenderer;

        private void Awake()
        {
            m_controller = GetComponent<WalkerController>();
            m_controller.onCellEnter += OnCellEnter;

            m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnCellEnter(Cell cell)
        {
            m_spriteRenderer.sortingOrder = cell.OrderInLayer;
        }

        private void OnDestroy()
        {
            m_controller.onCellEnter -= OnCellEnter;
        }
    }
}
