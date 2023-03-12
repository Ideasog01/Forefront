using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loadout", menuName = "Loadout")]
public class Loadout : ScriptableObject
{
    [SerializeField]
    private int[] generalSettingsValueArray;

    public int[] GeneralSettingsValueArray
    {
        get { return generalSettingsValueArray; }
        set { generalSettingsValueArray = value; }
    }
}

public struct Perk
{

};
