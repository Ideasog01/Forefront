using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class HighScoreInterface : MonoBehaviour
{
    [SerializeField]
    private HighScoreDisplay[] highScoreDisplay;

    [SerializeField]
    private GameSettings[] difficultyLevels;

    public void DisplayHighScores() //Via Inspector
    {
        for(int i = 0; i < highScoreDisplay.Length; i++)
        {
            HighScoreDisplay display = highScoreDisplay[i];
            GameSettings difficulty = difficultyLevels[i];

            display.ScoreText.text = difficulty.HighScore.ToString();
            display.StatisticText.text = "Hostiles Defeated: " + difficulty.HostilesDefeated.ToString() + "\nDeaths: " + difficulty.PlayerDeaths.ToString() + "\n\nCompleted: " + difficulty.DateCompleted;
        }
    }
}

[System.Serializable]
public struct HighScoreDisplay
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI statisticText;

    public TextMeshProUGUI ScoreText
    {
        get { return scoreText; }
    }

    public TextMeshProUGUI StatisticText
    {
        get { return statisticText; }
    }
}
