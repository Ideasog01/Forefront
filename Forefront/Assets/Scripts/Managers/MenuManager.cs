using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameSettings[] difficultyLevels;

    private ControllerManager _controllerManager;

    private void Awake()
    {
        _controllerManager = GameObject.Find("GameManager").GetComponent<ControllerManager>();
    }

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

    public void EnableCombatMode() //Via Inspector
    {
        _controllerManager.EnablePlasmaCannon(!PlayerEntity.plasmaActive);
    }

    private void LoadMainLevel()
    {
        SceneManager.LoadScene(1);
    }
}
