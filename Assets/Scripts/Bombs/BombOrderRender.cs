using GameField;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Bombs
{
    public class BombOrderRender : MonoBehaviour
    {
        private GameGrid m_grid;
        private SpriteRenderer m_spriteRenderer;

        [Inject]
        private void Constrcut(GameGrid grid)
        {
            m_grid = grid;
        }

        private void Awake()
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            m_spriteRenderer.sortingOrder = m_grid.GetCell(transform.position).RenderOrderInLayer;
        }
    }
}
