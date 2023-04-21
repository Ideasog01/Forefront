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

    public static PerkManager perkManager;

    public static Loadout mainLoadout;

    public static bool gameInProgress;

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

        gameInProgress = true;
    }

    private void InitialiseManager()
    {
        controllerManager = this.GetComponent<ControllerManager>();
        spawnManager = this.GetComponent<SpawnManager>();
        guiManager = this.GetComponent<GUIManager>();
        audioManager = this.GetComponent<AudioManager>();
        visualEffectManager = this.GetComponent<VisualEffectManager>();
        perkManager = this.GetComponent<PerkManager>();
        waveManager = GameObject.FindObjectOfType<WaveManager>(); //Find type as this is a prefab for multiple difficulty levels with different names

        playerEntity = GameObject.Find("XR Origin").GetComponent<PlayerEntity>();
    }

    public void ReturnMainMenu() //Via Inspector
    {
        WaveManager.missionFails = 0;
        SceneManager.LoadScene(0);
        Debug.Log("Return to Main Menu");
    }

    public void RestartGame() //Via Inspector
    {
        WaveManager.missionFails++;
        SceneManager.LoadScene(1);
        Debug.Log("Game Restarted");
    }
}
