using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectorController : MonoBehaviour
{
    [SerializeField]
    private string[] validObjNames;

    [SerializeField]
    private Image[] optionImages;

    [SerializeField]
    private Color selectColor;

    [SerializeField]
    private Color defaultColor;

    private int _selectOption;

    public void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < validObjNames.Length; i++)
        {
            if (other.gameObject.name == validObjNames[i]) //Identify the button
            {
                SelectOption(i);
                break;
            }
        }
    }

    private void SelectOption(int index) //Display active button and store index
    {
        _selectOption = index;

        for(int i = 0; i < optionImages.Length; i++)
        {
            if(i == index)
            {
                optionImages[i].color = selectColor;
            }
            else
            {
                optionImages[i].color = defaultColor;
            }
        }
    }

    public void ConfirmOption()
    {
        //0 = disable all, 1 = enable plasma cannon, 2 = cancel, 3 = enable pulse cannon
        GameManager.controllerManager.EnablePlasmaCannon(_selectOption == 0 && _selectOption != 1);
        GameManager.controllerManager.EnableLaser(_selectOption == 2 && _selectOption != 1);
    }
}
