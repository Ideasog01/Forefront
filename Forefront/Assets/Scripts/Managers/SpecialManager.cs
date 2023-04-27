using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialManager : MonoBehaviour
{
    public bool isSpecialActive;

    //[HideInInspector]
    public SpecialProperties activeSpecial;

    public enum SpecialType { Grenade, HealingStation, EMP, Turret, Blade, Laser}

    [Header("General")]

    [SerializeField]
    private SpecialProperties[] specialArray;

    [SerializeField]
    private GameObject leftHand;

    [Header("Display")]

    [SerializeField]
    private GameObject specialMenu;

    [SerializeField]
    private Image[] specialIcons;

    [SerializeField]
    private TextMeshProUGUI[] specialNames;

    [SerializeField]
    private Button nextWaveButton;

    [Header("Other")]

    [SerializeField]
    private int[] specialIndex;

    [Header("Special Objects")]

    private bool _specialSelected;

    #region SpecialSelection

    public void DisplaySpecialMenu()
    {
        specialMenu.SetActive(true);
        GameManager.guiManager.SetDisplayLocation(specialMenu.transform);

        for (int i = 0; i < specialNames.Length; i++)
        {
            Button button = specialNames[i].transform.parent.GetComponent<Button>();
            button.interactable = true;
        }

        nextWaveButton.interactable = false;

        //Select random specials for the player to choose from
        for (int i = 0; i < specialIndex.Length; i++)
        {
            int index = SelectRandomSpecial();
            specialIndex[i] = index;

            //Display the selected special

            specialIcons[i].sprite = specialArray[index].SpecialIcon;
            specialNames[i].text = specialArray[index].SpecialName;
        }
    }

    public void SelectSpecial(int index) //Index is from 0 - 2, as there are three options for the player to choose from
    {
        _specialSelected = true;
        nextWaveButton.interactable = true;

        activeSpecial = specialArray[specialIndex[index]];

        for (int i = 0; i < specialNames.Length; i++)
        {
            Button button = specialNames[i].transform.parent.GetComponent<Button>();

            if(index == i)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
        }

        GameManager.guiManager.PlayButtonPositiveSound();
    }

    public void NextWave()
    {
        if(_specialSelected)
        {
            WaveTrigger waveTrigger = GameManager.waveManager.waveArray[GameManager.waveManager.waveIndex].WaveTriggerRef; //A wave trigger is neccessary for a new room

            if(waveTrigger == null) //The wave will take place in the same room as the last one, so start the next wave now
            {
                GameManager.waveManager.BeginEncounter();
            }
            else
            {
                waveTrigger.gameObject.SetActive(true); //Wait for the player to travel to the next room
                GameManager.guiManager.DisplayBigMessage("TRAVEL TO NEXT ROOM");
            }

            GameManager.controllerManager.EnablePlasmaCannon(true);
            specialMenu.SetActive(false);
            isSpecialActive = true;
            _specialSelected = false;
        }
    }

    private int SelectRandomSpecial()
    {
        int randomIndex = Random.Range(0, (specialArray.Length)); 

        if(randomIndex > specialArray.Length - 1) //So the number last element is more common (not minus 1). In testing, the last value (blade special) was very rare.
        {
            randomIndex = specialArray.Length - 1;
        }

        return randomIndex;
    }

    #endregion

    #region SpecialActions

    public void ActivateSpecial()
    {
        if(isSpecialActive)
        {
            if(activeSpecial.SpecialObject != null)
            {
                activeSpecial.SpecialObject.SetActive(true);
                activeSpecial.SpecialObject.transform.position = activeSpecial.ObjectSpawn.position;
            }
            else
            {
                if(activeSpecial.SpecialTypeRef == SpecialType.Laser)
                {
                    GameManager.controllerManager.EnableLaser(true);
                    StartCoroutine(DelayDisableLaser());
                    leftHand.SetActive(false);
                }
            }

            isSpecialActive = false;
            Debug.Log("Special Activated");
        }
    }

    private IEnumerator DelayDisableLaser()
    {
        yield return new WaitForSeconds(8);
        GameManager.controllerManager.EnableLaser(false);
        leftHand.SetActive(true);
    }

    #endregion
}

[System.Serializable]
public struct SpecialProperties //The properties for each special ability
{
    [SerializeField]
    private string specialName;

    [SerializeField]
    private Sprite specialIcon;

    [SerializeField]
    private SpecialManager.SpecialType specialType;

    [SerializeField]
    private GameObject specialObject;

    [SerializeField]
    private Transform objectSpawn;

    public string SpecialName
    {
        get { return specialName; }
    }

    public Sprite SpecialIcon
    {
        get { return specialIcon; }
    }

    public SpecialManager.SpecialType SpecialTypeRef
    {
        get { return specialType; }
    }

    public GameObject SpecialObject
    {
        get { return specialObject; }
    }

    public Transform ObjectSpawn
    {
        get { return objectSpawn; }
    }
}
