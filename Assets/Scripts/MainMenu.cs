using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject m_bombPrefab;
    [SerializeField] float m_reloadBomb = 2.0f;
    [SerializeField] Image m_reloadImage;
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
        Instantiate(m_bombPrefab, MyGrid.Instance.GetPosition(PlayerController.Instance.Controller.CurrentCell), Quaternion.identity);
        StartCoroutine(ReloadBomb());
    }

    private IEnumerator ReloadBomb()
    {
        float timer = 0.0f;
        while(timer < m_reloadBomb)
        {
            yield return null;
            timer += Time.deltaTime;
            m_reloadImage.fillAmount = 1.0f - (timer / m_reloadBomb);
        }
        m_reloadImage.enabled = false;
    }
}
