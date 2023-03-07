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
    private int droneDamage;

    [SerializeField]
    private float droneDodgeCooldown;

    [SerializeField]
    private float droneAttackCooldown;

    public int DroneHealth
    {
        get { return droneHealth; }
    }

    public int DroneDamage
    {
        get { return droneDamage; }
    }

    public float DroneDodgeCooldown
    {
        get { return droneDodgeCooldown; }
    }

    public float DroneAttackCooldown
    {
        get { return droneAttackCooldown; }
    }

    public Transform SpawnPrefab
    {
        get { return spawnPrefab; }
    }
}
