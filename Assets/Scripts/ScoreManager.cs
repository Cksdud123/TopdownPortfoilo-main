using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public static ScoreManager instance;

    int score = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score : " + score.ToString();
    }
    public void AddPoint()
    {
        score += 10;
        scoreText.text = "Score : " + score.ToString();
    }
}
