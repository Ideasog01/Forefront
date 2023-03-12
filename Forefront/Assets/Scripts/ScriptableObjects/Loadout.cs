using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loadout", menuName = "Loadout")]
public class Loadout : ScriptableObject
{
    [SerializeField]
    private int[] generalSettingsValueArray;

    [SerializeField]
    private Perk perk1;

    [SerializeField]
    private Perk perk2;

    public int[] GeneralSettingsValueArray
    {
        get { return generalSettingsValueArray; }
        set { generalSettingsValueArray = value; }
    }

    public Perk Perk1
    {
        get { return perk1; }
        set { perk1 = value; }
    }

    public Perk Perk2
    {
        get { return perk2; }
        set { perk2 = value; }
    }
}
