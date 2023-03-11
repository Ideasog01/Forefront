using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameSettings[] difficultyLevels;

    public void SelectDifficulty(int index) //Via Inspector
    {
        GameManager.gameSettings = difficultyLevels[index]; //Select difficulty level
        LoadMainLevel();
    }

    public void ExitGame() //Via Inspector
    {
        Application.Quit();
        Debug.Log("EXIT GAME");
    }

    private void LoadMainLevel()
    {
        SceneManager.LoadScene(1);
    }
}
