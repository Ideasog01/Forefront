using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static ControllerManager controllerManager;

    public static SpawnManager spawnManager;

    public static PlayerEntity playerEntity;

    public static GUIManager guiManager;

    private void Awake()
    {
        InitialiseManager();
    }

    private void InitialiseManager()
    {
        controllerManager = this.GetComponent<ControllerManager>();
        spawnManager = this.GetComponent<SpawnManager>();
        guiManager = this.GetComponent<GUIManager>();

        playerEntity = GameObject.Find("XR Origin").GetComponent<PlayerEntity>();
    }
}
