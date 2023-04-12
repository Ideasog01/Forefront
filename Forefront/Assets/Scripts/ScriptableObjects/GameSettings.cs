using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    public enum DifficultyLevel { Recruit, Hero, Legend, Unrivalled };

    [SerializeField]
    private DifficultyLevel difficultyLevel;

    [SerializeField]
    private Transform spawnPrefab;

    [Header("Drone Properties")]

    [SerializeField]
    private int droneHealth;

    [SerializeField]
    private Transform droneProjectilePrefab;

    [SerializeField]
    private float droneAttackCooldown;

    [Header("High Score & Statistics")]

    [SerializeField]
    private int highScore;

    [SerializeField]
    private int hostilesDefeated;

    [SerializeField]
    private int playerDeaths;

    [SerializeField]
    private string dateCompleted;

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

    public Transform SpawnPrefab
    {
        get { return spawnPrefab; }
    }

    public int HighScore
    {
        get { return highScore; }
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
