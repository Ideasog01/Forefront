using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using System.Collections;

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

    [Header("Shield Display")]

    [SerializeField]
    private Slider shieldSlider;

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

    [SerializeField]
    private Sound damageCriticalSound;

    [Header("UI Sounds")]

    [SerializeField]
    private Sound positiveButtonSound;

    [SerializeField]
    private Sound negativeButtonSound;

    [Header("Defeat Canvas")]

    [SerializeField]
    private GameObject defeatCanvas;

    [SerializeField]
    private Sound defeatSound;

    [SerializeField]
    private TextMeshProUGUI defeatScoreText;

    [SerializeField]
    private TextMeshProUGUI defeatHighScoreText;

    [Header("Victory Canvas")]

    [SerializeField]
    private GameObject victoryCanvas;

    [SerializeField]
    private TextMeshProUGUI victoryStatisticsText;

    [SerializeField]
    private TextMeshProUGUI victoryAdditionalDetailsText;

    [SerializeField]
    private Sound victorySound;

    [Header("WaveCompleteScreen")]

    [SerializeField]
    private Sound waveCompleteSound;

    [Header("Big Message Display")]

    [SerializeField]
    private Animator bigMessageAnimator;

    [SerializeField]
    private TextMeshProUGUI bigMessageText;

    [Header("Other")]

    public Sound multiEnemyDefeatSound;

    public Sound nextWaveSound;

    [SerializeField]
    private Transform displaySpawn; //The position of where the victory, defeat and wave complete canvas should spawn

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
        scoreText.text = "Score: " + GameManager.waveManager.playerScore.ToString();
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

        if (health < 30 && !damageCriticalText.gameObject.activeSelf)
        {
            GameManager.audioManager.PlaySound(damageCriticalSound);
        }

        damageCriticalText.gameObject.SetActive(health < 30 && health > 0);

        if(health <= 0)
        {
            GameManager.controllerManager.DisplayTeleportRay(false);
            DisplayDefeatCanvas();
            GameManager.gameInProgress = false;
        }
    }

    public void DisplayVictoryCanvas() //Display/Position the canvas and game statistics
    {
        victoryCanvas.SetActive(true);
        victoryStatisticsText.text = "SCORE: " + GameManager.waveManager.playerScore.ToString() + "\nHOSTILES DEFEATED: " + GameManager.waveManager.hostilesDefeated.ToString()
            + "\nMISSION FAILS: " + WaveManager.missionFails.ToString();

        SetDisplayLocation(victoryCanvas.transform);
        
        victoryAdditionalDetailsText.text = "Level: " + SceneManager.GetActiveScene().name + "\nHigh Score: " + GameManager.gameSettings.HighScore.ToString();
        GameManager.audioManager.PlaySound(victorySound);
    }

    public void DisplayDefeatCanvas() //Display/Position the canvas
    {
        if (GameManager.waveManager.playerScore > GameManager.gameSettings.HighScore)
        {
            GameManager.gameSettings.HighScore = GameManager.waveManager.playerScore;
        }

        defeatCanvas.SetActive(true);
        defeatScoreText.text = "Score: " + GameManager.waveManager.playerScore.ToString();
        defeatHighScoreText.text = "High Score: " + GameManager.gameSettings.HighScore.ToString();

        SetDisplayLocation(defeatCanvas.transform);

        GameManager.audioManager.PlaySound(defeatSound);
    }

    public void PlayButtonPositiveSound() //Via Inspector
    {
        GameManager.audioManager.PlaySound(positiveButtonSound);
    }

    public void PlayButtonNegativeSound() //Via Inspector
    {
        GameManager.audioManager.PlaySound(negativeButtonSound);
    }

    public void DisplayShieldHealth(float health, float maxHealth)
    {
        shieldSlider.maxValue = maxHealth;
        shieldSlider.value = health;
    }

    public void DisplayWaveCompleteNotification()
    {
        DisplayBigMessage("Wave Complete");
        GameManager.audioManager.PlaySound(waveCompleteSound);
    }

    public void DisplayBigMessage(string messageContent)
    {
        bigMessageText.text = messageContent;
        bigMessageAnimator.SetTrigger("active");
    }

    public void SetDisplayLocation(Transform display)
    {
        display.position = displaySpawn.position;
        display.rotation = displaySpawn.rotation;
        display.eulerAngles = new Vector3(0, displaySpawn.eulerAngles.y, 0);
    }
}
