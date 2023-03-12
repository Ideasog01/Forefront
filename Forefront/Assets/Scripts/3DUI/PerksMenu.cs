using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerksMenu : MonoBehaviour
{
    [Header("Perks")]

    [SerializeField]
    private GameObject perkGrabbable;

    [SerializeField]
    private Perk[] perkArray;

    [Header("Perk Display")]

    [SerializeField]
    private TextMeshProUGUI perkNameText;

    [SerializeField]
    private TextMeshProUGUI perkDescriptionText;

    [SerializeField]
    private Material perkIcon;

    [SerializeField]
    private Image perkSlot1;

    [SerializeField]
    private Image perkSlot2;

    [SerializeField]
    private Image[] perkIconsArray;

    [Header("Loadout")]

    [SerializeField]
    private Loadout mainLoadout;

    private Vector3 _perkGrabbableInitialLocalPos;
    private Quaternion _perkGrabbableInitialLocalRot;

    private int _selectedPerk;

    private void Awake()
    {
        _perkGrabbableInitialLocalPos = perkGrabbable.transform.localPosition;
        _perkGrabbableInitialLocalRot = perkGrabbable.transform.localRotation;

        for(int i = 0; i < perkIconsArray.Length; i++)
        {
            perkIconsArray[i].sprite = perkArray[i].PerkIcon;
        }

        perkIcon.mainTexture = null;

        DisplaySlot();
    }

    public void PerkReleased() //Via Inspector
    {

        //Is near a slot? Then assign it.

        float distanceToPerkSlot1 = Vector3.Distance(perkGrabbable.transform.position, perkSlot1.transform.position);
        float distanceToPerkSlot2 = Vector3.Distance(perkGrabbable.transform.position, perkSlot2.transform.position);

        if (distanceToPerkSlot1 < 0.1f)
        {
            //Avoids the perk being equipped in both slots
            if (mainLoadout.Perk2 != perkArray[_selectedPerk])
            {
                if (mainLoadout.Perk1 != null) //Unequip the current perk if it is valid
                {
                    mainLoadout.Perk1.IsActive = false;
                }

                mainLoadout.Perk1 = perkArray[_selectedPerk];
                perkArray[_selectedPerk].IsActive = true;
            }
        }
        else
        {
            if (distanceToPerkSlot2 < 0.1f)
            {
                //Avoids the perk being equipped in both slots
                if (mainLoadout.Perk1 != perkArray[_selectedPerk]) //Unequip the current perk if it is valid
                {
                    if (mainLoadout.Perk2 != null)
                    {
                        mainLoadout.Perk2.IsActive = false;
                    }

                    mainLoadout.Perk2 = perkArray[_selectedPerk];
                    perkArray[_selectedPerk].IsActive = true;
                }
            }
        }

        DisplaySlot();

        //Set grabbable position to local origin

        perkGrabbable.transform.localPosition = _perkGrabbableInitialLocalPos;
        perkGrabbable.transform.localRotation = _perkGrabbableInitialLocalRot;

    }

    public void SelectPerk(int index) //Via Inspector
    {
        _selectedPerk = index;

        perkNameText.text = perkArray[_selectedPerk].PerkName;
        perkDescriptionText.text = perkArray[_selectedPerk].PerkDescription;
        perkIcon.mainTexture = perkArray[_selectedPerk].PerkIcon.texture;
    }

    public void DisplaySlot()
    {
        if(mainLoadout.Perk1 != null)
        {
            perkSlot1.sprite = mainLoadout.Perk1.PerkIcon;
        }

        if(mainLoadout.Perk2 != null)
        {
            perkSlot2.sprite = mainLoadout.Perk2.PerkIcon;
        }
    }
}
