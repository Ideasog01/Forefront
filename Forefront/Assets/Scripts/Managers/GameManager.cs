using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static ControllerManager controllerManager;

    public static SpawnManager spawnManager;

    public static PlayerEntity playerEntity;

    public static GUIManager guiManager;

    public static GameSettings gameSettings;

    public static WaveManager waveManager;

    public static AudioManager audioManager;

    public static VisualEffectManager visualEffectManager;

    public static Loadout mainLoadout;

    [SerializeField]
    private Loadout mainLoadoutRef;

    [SerializeField]
    private GameSettings testSettings;

    private void Awake()
    {
        if (gameSettings == null)
        {
            gameSettings = testSettings;
        }

        mainLoadout = mainLoadoutRef;

        InitialiseManager();
    }

    private void InitialiseManager()
    {
        controllerManager = this.GetComponent<ControllerManager>();
        spawnManager = this.GetComponent<SpawnManager>();
        guiManager = this.GetComponent<GUIManager>();
        audioManager = this.GetComponent<AudioManager>();
        visualEffectManager = this.GetComponent<VisualEffectManager>();
        waveManager = GameObject.FindObjectOfType<WaveManager>(); //Find type as this is a prefab for multiple difficulty levels with different names

        playerEntity = GameObject.Find("XR Origin").GetComponent<PlayerEntity>();
    }

    public void ReturnMainMenu() //Via Inspector
    {
        SceneManager.LoadScene(0);
        Debug.Log("Return to Main Menu");
    }

    public void RestartGame() //Via Inspector
    {
        SceneManager.LoadScene(1);
        Debug.Log("Game Restarted");
    }
}
