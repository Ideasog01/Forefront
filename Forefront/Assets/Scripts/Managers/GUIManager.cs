using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

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

    [Header("UI Sounds")]

    [SerializeField]
    private Sound positiveButtonSound;

    [SerializeField]
    private Sound negativeButtonSound;

    [Header("Defeat Canvas")]

    [SerializeField]
    private GameObject defeatCanvas;

    [Header("Victory Canvas")]

    [SerializeField]
    private GameObject victoryCanvas;

    [SerializeField]
    private TextMeshProUGUI victoryStatisticsText;

    [SerializeField]
    private TextMeshProUGUI victoryAdditionalDetailsText;

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
            float charge = (PlayerEntity.plasmaCharge / GameManager.playerEntity.PlasmaChargeTime) * 100;

            plasmaChargeText.text = "Plasma Charge: " + charge.ToString("F0") + "%";
            plasmaChargeSlider.maxValue = 100;
            plasmaChargeSlider.value = charge;
        }
    }

    public void TogglePlasmaCharge(bool active)
    {
        plasmaChargeCanvas.SetActive(active);
    }

    public void DisplayHostileCount()
    {
        hostilesRemainingText.text = "Hostiles Remaining: " + SpawnManager.activeHostiles.ToString();
    }

    public void DisplayScore()
    {
        scoreText.text = "Score: " + PlayerEntity.scoreAmount.ToString();
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

    public void DisplayPlayerHealth() //Via Inspector
    {
        int health = GameManager.playerEntity.EntityHealth;

        playerHealthSlider.maxValue = GameManager.playerEntity.EntityMaxHealth;
        playerHealthSlider.value = health;

        playerHealthText.text = health.ToString();

        damageCriticalText.gameObject.SetActive(health < 30);

        if(health <= 0)
        {
            DisplayDefeatCanvas();
        }
    }

    public void DisplayVictoryCanvas() //Display/Position the canvas and game statistics
    {
        victoryCanvas.SetActive(true);
        victoryStatisticsText.text = "SCORE: " + GameManager.waveManager.playerScore.ToString() + "\nHOSTILES DEFEATED: " + GameManager.waveManager.hostilesDefeated.ToString()
            + "\nMISSION FAILS: " + WaveManager.missionFails.ToString();
        victoryCanvas.transform.position = GameObject.Find("Main Camera").transform.position + new Vector3(0, 1, 2);
        victoryAdditionalDetailsText.text = "Level: " + SceneManager.GetActiveScene().name + "\nHigh Score: " + GameManager.gameSettings.HighScore.ToString();
    }

    public void DisplayDefeatCanvas() //Display/Position the canvas
    {
        defeatCanvas.SetActive(true);
        defeatCanvas.transform.position = GameObject.Find("Main Camera").transform.position + new Vector3(0, 1, 2);
    }

    public void PlayButtonPositiveSound() //Via Inspector
    {
        GameManager.audioManager.PlaySound(positiveButtonSound);
    }

    public void PlayButtonNegativeSound() //Via Inspector
    {
        GameManager.audioManager.PlaySound(negativeButtonSound);
    }
}
