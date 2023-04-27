using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class HighScoreInterface : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] scoreText;

    [SerializeField]
    private GameSettings[] difficultyLevels;

    public void DisplayHighScores() //Via Inspector
    {
        for(int i = 0; i < scoreText.Length; i++)
        {
            GameSettings difficulty = difficultyLevels[i];
            scoreText[i].text = difficulty.HighScore.ToString();
        }
    }
}
