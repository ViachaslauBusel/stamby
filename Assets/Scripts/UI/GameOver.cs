using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] Text m_label;
        private Canvas m_canvas;

        private void Awake()
        {
            m_canvas = GetComponent<Canvas>();
            m_canvas.enabled = false;
        }

        public void Open(float delay, string label = "GAME OVER")
        {
            m_label.text = label;

            StartCoroutine(OpenDelay(delay));
        }


        private IEnumerator OpenDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Time.timeScale = 0.0f;
            m_canvas.enabled = true;
        }
    }
}