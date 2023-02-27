using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static ControllerManager controllerManager;

    private void Awake()
    {
        InitialiseManager();
    }

    private void InitialiseManager()
    {
        controllerManager = this.GetComponent<ControllerManager>();
    }
}
