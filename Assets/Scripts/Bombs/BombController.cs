using GameField;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Bombs
{
    public class BombController : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_bombPrefab;
        [SerializeField]
        private float m_reloadBomb = 2.0f;
        [SerializeField]
        private Image m_reloadImage;
        private DiContainer m_container;
        private GameGrid m_grid;
        private PlayerController m_playerController;

        [Inject]
        private void Construct(DiContainer container, GameGrid myGrid, PlayerController playerController)
        {
            m_container = container;
            m_grid = myGrid;
            m_playerController = playerController;
        }

        public void ThrowBomb()
        {
            if (m_reloadImage.enabled) return;
            m_reloadImage.enabled = true;
            m_reloadImage.fillAmount = 1.0f;
            m_container.InstantiatePrefab(m_bombPrefab, m_grid.GetPosition(m_playerController.Controller.CurrentCell), Quaternion.identity, null);
            StartCoroutine(ReloadBomb());
        }

        private IEnumerator ReloadBomb()
        {
            float timer = 0.0f;
            while (timer < m_reloadBomb)
            {
                yield return null;
                timer += Time.deltaTime;
                m_reloadImage.fillAmount = 1.0f - (timer / m_reloadBomb);
            }
            m_reloadImage.enabled = false;
        }
    }
}
