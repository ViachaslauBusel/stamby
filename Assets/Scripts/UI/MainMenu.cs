using GameField;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;


namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] GameObject m_bombPrefab;
        [SerializeField] float m_reloadBomb = 2.0f;
        [SerializeField] Image m_reloadImage;
        private GameGrid m_myGrid;
        private PlayerController m_playerController;

        [Inject]
        private void Construct(GameGrid myGrid, PlayerController playerController)
        {
            m_myGrid = myGrid;
            m_playerController = playerController;
        }
        public void ResetScene()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("SampleScene");
        }

        public void ThrowBomb()
        {
            if (m_reloadImage.enabled) return;
            m_reloadImage.enabled = true;
            m_reloadImage.fillAmount = 1.0f;
            Instantiate(m_bombPrefab, m_myGrid.GetPosition(m_playerController.Controller.CurrentCell), Quaternion.identity);
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