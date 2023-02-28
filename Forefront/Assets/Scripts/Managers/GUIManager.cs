using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIManager : MonoBehaviour
{
    [Header("Plasma Charge")]

    [SerializeField]
    private GameObject plasmaChargeCanvas;

    [SerializeField]
    private TextMeshProUGUI plasmaChargeText;

    [SerializeField]
    private Slider plasmaChargeSlider;

    public void DisplayPlasmaCharge()
    {
        if(plasmaChargeCanvas.activeSelf)
        {
            float charge = PlayerEntity.plasmaCharge;

            plasmaChargeText.text = "Plasma Charge: " + charge.ToString("F0") + "%";
            plasmaChargeSlider.value = charge;
        }
    }

    public void TogglePlasmaCharge(bool active)
    {
        plasmaChargeCanvas.SetActive(active);
    }
}
