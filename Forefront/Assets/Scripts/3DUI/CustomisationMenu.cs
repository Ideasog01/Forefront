using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomisationMenu : MonoBehaviour
{
    [Header("General")]

    [SerializeField]
    private GameObject rightHandController;

    [Header("Menu Objects")]

    [SerializeField]
    private GameObject generalSettingsMenu;

    [SerializeField]
    private GameObject perksMenu;

    [SerializeField]
    private GameObject loadoutMenu;

    public void SpawnGeneralSettingsMenu() //Via Inspector
    {
        generalSettingsMenu.SetActive(true);
        generalSettingsMenu.transform.position = rightHandController.transform.position;
    }

    public void SpawnPerksMenu()
    {
        perksMenu.SetActive(true);
        perksMenu.transform.position = rightHandController.transform.position;
    }

    public void SpawnLoadoutMenus()
    {
        loadoutMenu.SetActive(true);
        loadoutMenu.transform.position = rightHandController.transform.position;
    }
}
