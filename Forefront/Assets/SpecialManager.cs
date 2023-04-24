using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialManager : MonoBehaviour
{
    public SpecialProperties selectedSpecial;

    [Header("General")]

    [SerializeField]
    private SpecialProperties[] specialArray;

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
    private Sound nextWaveSound;

    [SerializeField]
    private int[] specialIndex;

    private bool _specialSelected;

    public void DisplaySpecialMenu()
    {
        specialMenu.SetActive(true);
        specialMenu.transform.position = GameObject.Find("Main Camera").transform.position + new Vector3(0, 1, 2);

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
        selectedSpecial = specialArray[specialIndex[index]];
        _specialSelected = true;
        nextWaveButton.interactable = true;

        for(int i = 0; i < specialNames.Length; i++)
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
            GameManager.audioManager.PlaySound(nextWaveSound);
            GameManager.waveManager.BeginEncounter();
            specialMenu.SetActive(false);
            _specialSelected = false;
        }
    }

    private int SelectRandomSpecial()
    {
        int randomIndex = Random.Range(0, specialArray.Length - 1);
        return randomIndex;
    }
}

[System.Serializable]
public struct SpecialProperties
{
    [SerializeField]
    private string specialName;

    [SerializeField]
    private Sprite specialIcon;

    public string SpecialName
    {
        get { return specialName; }
    }

    public Sprite SpecialIcon
    {
        get { return specialIcon; }
    }
}
