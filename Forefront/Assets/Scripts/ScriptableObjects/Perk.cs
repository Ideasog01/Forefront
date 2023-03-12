using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Perk", menuName = "Perk")]
public class Perk : ScriptableObject
{
    [SerializeField]
    private string perkName;

    [SerializeField]
    private string perkDescription;

    [SerializeField]
    private Sprite perkIcon;

    [SerializeField]
    private bool isActive;

    public string PerkName
    {
        get { return perkName; }
    }

    public string PerkDescription
    {
        get { return perkDescription; }
    }

    public Sprite PerkIcon
    {
        get { return perkIcon; }
    }

    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }
};
