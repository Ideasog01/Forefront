using System.Net.NetworkInformation;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    public enum DifficultyLevel { Recruit, Hero, Legend, Unrivalled };

    [SerializeField]
    private DifficultyLevel difficultyLevel;

    [SerializeField]
    private Transform spawnPrefab;

    [Header("Shield Properties")]

    [SerializeField]
    private float shieldChargeRate;

    [SerializeField]
    private float shieldCooldown; //The amount of time the shield is disabled after its health reaches <= 0

    [SerializeField]
    private float shieldMaxHealth;

    [SerializeField]
    private float shieldRegenerateWaitTime; //The time to wait after damage to regenerate the shield

    [Header("Drone Properties")]

    [SerializeField]
    private int droneHealth;

    [SerializeField]
    private Transform droneProjectilePrefab;

    [SerializeField]
    private float droneAttackCooldown;

    [SerializeField]
    private int droneScoreAmount;

    [Header("Exploder Properties")]

    [SerializeField]
    private int exploderHealth;

    [SerializeField]
    private int exploderAttackDamage;

    [SerializeField]
    private int exploderScoreAmount;

    [Header("Tank Properties")]

    [SerializeField]
    private int tankScoreAmount;

    [SerializeField]
    private Transform tankProjectilePrefab;

    [SerializeField]
    private int tankHealth;

    [SerializeField]
    private float tankAttackCooldown;

    [Header("High Score & Statistics")]

    [SerializeField]
    private int highScore;

    [SerializeField]
    private int hostilesDefeated;

    [SerializeField]
    private int playerDeaths;

    [SerializeField]
    private string dateCompleted;

    //Drone Properties

    public int DroneHealth
    {
        get { return droneHealth; }
    }

    public float DroneAttackCooldown
    {
        get { return droneAttackCooldown; }
    }

    public Transform DroneProjectilePrefab
    {
        get { return droneProjectilePrefab; }
    }

    public int DroneScoreAmount
    {
        get { return droneScoreAmount; }
    }

    //Exploder Properties

    public int ExploderHealth
    {
        get { return exploderHealth; }
    }

    public int ExploderAttackDamage
    {
        get { return exploderAttackDamage; }
    }

    public int ExploderScoreAmount
    {
        get { return exploderScoreAmount; }
    }

    //Tank Properties

    public int TankScoreAmount
    {
        get { return tankScoreAmount; }
    }

    public Transform TankProjectilePrefab
    {
        get { return tankProjectilePrefab; }
    }

    public int TankHealth
    {
        get { return tankHealth; }
        set { tankHealth = value; }
    }

    public float TankAttackCooldown
    {
        get { return tankAttackCooldown; }
    }

    //Shield

    public float ShieldChargeRate
    {
        get { return shieldChargeRate; }
    }

    public float ShieldCooldown
    {
        get { return shieldCooldown; }
    }

    public float ShieldMaxHealth
    {
        get { return shieldMaxHealth; }
    }

    public float ShieldRegenerateWaitTime
    {
        get { return shieldRegenerateWaitTime; }
    }

    //Other

    public Transform SpawnPrefab
    {
        get { return spawnPrefab; }
    }

    public int HighScore
    {
        get { return highScore; }
        set { highScore = value; }
    }

    public int HostilesDefeated
    {
        get { return hostilesDefeated; }
    }

    public int PlayerDeaths
    {
        get { return playerDeaths; }
    }

    public string DateCompleted
    {
        get { return dateCompleted; }
    }
}
