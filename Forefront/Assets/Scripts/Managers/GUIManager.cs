using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class GUIManager : MonoBehaviour
{
    [Header("Plasma Charge")]

    [SerializeField]
    private GameObject plasmaChargeCanvas;

    [SerializeField]
    private TextMeshProUGUI plasmaChargeText;

    [SerializeField]
    private Slider plasmaChargeSlider;

    [Header("Wrist Display")]

    [SerializeField]
    private TextMeshProUGUI hostilesRemainingText;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI powerChargeText;

    [SerializeField]
    private Slider powerChargeSlider;

    [Header("Movement Transition")]

    [SerializeField]
    private Animator movementTransitionAnimator;

    [SerializeField]
    private TeleportationProvider teleportationProvider;

    [Header("Player Health Display")]

    [SerializeField]
    private Slider playerHealthSlider;

    [SerializeField]
    private TextMeshProUGUI playerHealthText;

    [SerializeField]
    private GameObject damageCriticalText;

    private void Start()
    {
        teleportationProvider.beginLocomotion += ctx => PerformTransition();
    }

    private void Update()
    {
        DisplayPowerCharge();
    }

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

    public void DisplayHostileCount()
    {
        hostilesRemainingText.text = SpawnManager.activeHostiles.ToString();
    }

    public void DisplayScore()
    {
        scoreText.text = PlayerEntity.scoreAmount.ToString();
    }

    public void DisplayPowerCharge()
    {
        powerChargeText.text = "Power Charge: " + PlayerEntity.powerChargeAmount.ToString("F0") + "%";
        powerChargeSlider.value = PlayerEntity.powerChargeAmount;
    }

    public void PerformTransition()
    {
        movementTransitionAnimator.SetTrigger("transition");
    }

    public void DisplayPlayerHealth()
    {
        int health = GameManager.playerEntity.EntityHealth;

        playerHealthSlider.value = health;
        playerHealthText.text = health.ToString();

        damageCriticalText.gameObject.SetActive(health < 30);
    }
}
