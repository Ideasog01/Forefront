using TMPro;
using UnityEngine;
using UnityEngine.UI;

//General settings refer to the properties of the hand cannon

public class GeneralSettings : MonoBehaviour
{
    [Header("Loadouts")]

    [SerializeField]
    private Loadout mainLoadout;

    [SerializeField]
    private Loadout[] loadoutArray;

    [Header("UI Elements")]

    [SerializeField]
    private Slider[] valueSliderArray;

    [SerializeField]
    private TextMeshProUGUI[] valueTextArray;

    [SerializeField]
    private TextMeshProUGUI totalPowerChargeText;

    [Header("Loadout Display")]

    [SerializeField]
    private TextMeshProUGUI slotNameText;

    [SerializeField]
    private PerksMenu perkMenu;

    private Loadout _selectedLoadout;

    public void DisplaySettingValues() //Via Inspector
    {
        int totalPowerUsed = 0;

        for (int i = 0; i < valueSliderArray.Length; i++)
        {
            totalPowerUsed += mainLoadout.GeneralSettingsValueArray[i]; //Get the total power being used
            valueSliderArray[i].value = mainLoadout.GeneralSettingsValueArray[i];
            valueTextArray[i].text = mainLoadout.GeneralSettingsValueArray[i].ToString() + "/5";
        }

        totalPowerChargeText.text = totalPowerUsed.ToString() + "/10";
    }

    public void ChangeSettingValue(int sliderIndex) //Via Inspector
    {
        int totalPowerUsed = 0;

        for (int i = 0; i < valueSliderArray.Length; i++) //Get the total power being used
        {
            totalPowerUsed += mainLoadout.GeneralSettingsValueArray[i];
        }

        int sliderValue = (int)valueSliderArray[sliderIndex].value;
        int sliderDif = sliderValue - mainLoadout.GeneralSettingsValueArray[sliderIndex];

        if (totalPowerUsed >= 10 && sliderDif > 0) //Power exceeds max, and slider value change is an increase, stop any increase
        {
            valueSliderArray[sliderIndex].value = mainLoadout.GeneralSettingsValueArray[sliderIndex];
        }
        else //Value change is valid, so update internal values
        {
            mainLoadout.GeneralSettingsValueArray[sliderIndex] = sliderValue;
            valueTextArray[sliderIndex].text = sliderValue.ToString() + "/5";

            totalPowerUsed = 0;

            //Display new total power
            for (int i = 0; i < valueSliderArray.Length; i++) //Get the total power being used
            {
                totalPowerUsed += mainLoadout.GeneralSettingsValueArray[i];
            }

            totalPowerChargeText.text = totalPowerUsed.ToString() + "/10";
        }
    }

    public void SelectSlot(int index) //Via Inspector
    {
        _selectedLoadout = loadoutArray[index];
        slotNameText.text = "Loadout " + (index + 1);
    }

    public void EquipLoadoutSlot() //Via Inspector
    {
        if(_selectedLoadout != null)
        {
            for(int i = 0; i < mainLoadout.GeneralSettingsValueArray.Length; i++)
            {
                mainLoadout.GeneralSettingsValueArray[i] = _selectedLoadout.GeneralSettingsValueArray[i];
            }

            if(mainLoadout.Perk1 != null && _selectedLoadout.Perk1 != null)
            {
                mainLoadout.Perk1 = _selectedLoadout.Perk1;
            }

            if (mainLoadout.Perk2 != null && _selectedLoadout.Perk2 != null)
            {
                mainLoadout.Perk2 = _selectedLoadout.Perk2;
            }

            //Checks if objects are active before modifying display

            if (valueSliderArray[0].isActiveAndEnabled)
            {
                DisplaySettingValues();
            }
            
            if(perkMenu.isActiveAndEnabled)
            {
                perkMenu.DisplaySlot();
            }
            
        }
    }

    public void OverwriteLoadoutSlot() //Via Inspector
    {
        if (_selectedLoadout != null)
        {
            for (int i = 0; i < mainLoadout.GeneralSettingsValueArray.Length; i++)
            {
                _selectedLoadout.GeneralSettingsValueArray[i] = mainLoadout.GeneralSettingsValueArray[i];
            }

            if (mainLoadout.Perk1 != null && _selectedLoadout.Perk1 != null)
            {
                _selectedLoadout.Perk1 = mainLoadout.Perk1;
            }

            if (mainLoadout.Perk2 != null && _selectedLoadout.Perk2 != null)
            {
                _selectedLoadout.Perk2 = mainLoadout.Perk2;
            }
        }
    }
}
