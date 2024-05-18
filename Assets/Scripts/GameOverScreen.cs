using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public void SetUp()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    public void RestartButton()
    {
        SceneManager.LoadScene("TopDownScene");
        Time.timeScale = 1.0f;
    }
    public void QuitButton()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void GameStart()
    {
        SceneManager.LoadScene("TopDownScene");
    }
    public void GameQuit()
    {
        Application.Quit();
    }
}
