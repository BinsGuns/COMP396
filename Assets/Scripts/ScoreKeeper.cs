using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score = 1000;

    void Start()
    {
        UpdateScoreText();
        StartCoroutine(DecreaseScoreOverTime());
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    IEnumerator DecreaseScoreOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            DecreaseScore(10);
        }
    }

    public void IncreaseScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    void DecreaseScore(int points)
    {
        score -= points;
        UpdateScoreText();
    }
}
