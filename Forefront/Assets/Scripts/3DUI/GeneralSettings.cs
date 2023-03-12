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

    public void SelectSlot(int index)
    {
        _selectedLoadout = loadoutArray[index];
        slotNameText.text = "Loadout " + (index + 1);
    }

    public void EquipLoadoutSlot()
    {
        if(_selectedLoadout != null)
        {
            mainLoadout.GeneralSettingsValueArray = _selectedLoadout.GeneralSettingsValueArray;

            if (valueSliderArray[0].isActiveAndEnabled) //Is general settings menu enabled?
            {
                DisplaySettingValues();
            }
        }
    }

    public void OverwriteLoadoutSlot()
    {
        if (_selectedLoadout != null)
        {
            _selectedLoadout.GeneralSettingsValueArray = mainLoadout.GeneralSettingsValueArray;

            if (valueSliderArray[0].isActiveAndEnabled) //Is general settings menu enabled?
            {
                DisplaySettingValues();
            }
        }
    }
}
