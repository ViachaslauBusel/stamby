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
        public void ResetScene()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("SampleScene");
        }
    }
}