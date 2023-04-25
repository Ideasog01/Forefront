using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using static EnemyEntity;

public class WaveManager : MonoBehaviour
{
    public static int missionFails; //static variable as seen is reset on player defeat. (Needs to be reset on return to main menu)

    public int waveIndex; //The current wave the player is on (Starts at wave 0)

    public int spawnIndex; //The current enemy being spawned

    public int playerScore;

    public int hostilesDefeated;

    public WaveDetails[] waveArray;

    private bool _enemiesSpawned;

    private bool _encounterInProgress;

    private int _recentEnemyDefeats;

    public void BeginEncounter()
    {
        if(!_encounterInProgress && waveIndex < waveArray.Length)
        {
            spawnIndex = 0;
            _encounterInProgress = true;

            GameManager.audioManager.PlaySound(GameManager.guiManager.nextWaveSound);

            StartCoroutine(DelaySpawn());
        }
    }

    public void EnemyDefeated(EnemyType type)
    {
        SpawnManager.activeHostiles--;
        GameManager.waveManager.hostilesDefeated++;

        Debug.Log(SpawnManager.activeHostiles);

        switch (type)
        {
            case EnemyType.Drone:
                GameManager.waveManager.playerScore += GameManager.gameSettings.DroneScoreAmount;
                break;
            case EnemyType.Exploder:
                GameManager.waveManager.playerScore += GameManager.gameSettings.ExploderScoreAmount;
                break;
            case EnemyType.Tank:
                GameManager.waveManager.playerScore += GameManager.gameSettings.TankScoreAmount;
                break;
        }

        GameManager.guiManager.DisplayScore();

        _recentEnemyDefeats++;

        if(_recentEnemyDefeats >= 4)
        {
            Debug.Log("Recent Enemy Defeats: " + _recentEnemyDefeats);
            GameManager.audioManager.PlaySound(GameManager.guiManager.multiEnemyDefeatSound);
            GameManager.guiManager.DisplayBigMessage("Quadruple Elimination!");
        }

        StartCoroutine(ResetRecentEnemyDefeats());

    }

    private IEnumerator ResetRecentEnemyDefeats()
    {
        int recentOld = _recentEnemyDefeats;
        yield return new WaitForSeconds(1.5f);
        if(_recentEnemyDefeats <= recentOld) //Then player defeated enemy within time
        {
            _recentEnemyDefeats = 0;
        }
    }

    private void Update()
    {
        //If all enemies have been spawned this wave, and all enemies have been defeated, the wave is complete!
        if(_enemiesSpawned)
        {
            if(SpawnManager.activeHostiles == 0)
            {
                if(waveIndex < waveArray.Length - 1)
                {
                    _encounterInProgress = false;
                    Debug.Log("Wave Complete!");
                    GameManager.guiManager.DisplayWaveCompleteNotification();
                    GameManager.playerEntity.EntityHealth = GameManager.playerEntity.EntityMaxHealth;
                    GameManager.guiManager.DisplayPlayerHealth();
                    GameManager.specialManager.DisplaySpecialMenu();
                    GameManager.controllerManager.EnablePlasmaCannon(false);
                    waveIndex++;
                }
                else
                {
                    EndGame();
                }

                _enemiesSpawned = false;
            }
        }
    }

    private void EndGame()
    {
        if(playerScore > GameManager.gameSettings.HighScore)
        {
            GameManager.gameSettings.HighScore = playerScore;
        }

        GameManager.playerEntity.Heal(GameManager.playerEntity.EntityMaxHealth);

        GameManager.guiManager.DisplayVictoryCanvas();
        Debug.Log("GameMode Ended");
    }

    private IEnumerator DelaySpawn()
    {
        WaveDetails currentWave = waveArray[waveIndex];

        yield return new WaitForSeconds(currentWave.SpawnSettingsArray[spawnIndex].SpawnCooldown); //Wait for the cooldown

        yield return new WaitUntil(() => SpawnManager.activeHostiles < currentWave.MaxEnemyCount); //Prevents too many enemies from being active

        GameManager.spawnManager.SpawnEnemy(currentWave.SpawnSettingsArray[spawnIndex]);

        Debug.Log("Enemy Spawned!");

        if(spawnIndex + 1 < currentWave.SpawnSettingsArray.Length) //Is NOT the end of the wave
        {
            spawnIndex++;
            StartCoroutine(DelaySpawn());
        }
        else
        {
            _enemiesSpawned = true; //Allows for the game to wait until all enemies have been defeated
            Debug.Log("All enemies spawned!");
        }
    }
}

[System.Serializable]
public struct WaveDetails
{
    [SerializeField]
    private SpawnSettings[] spawnSettingsArray;

    [SerializeField]
    private int maxEnemyCount;

    [SerializeField]
    private WaveTrigger waveTrigger;

    public SpawnSettings[] SpawnSettingsArray
    {
        get { return spawnSettingsArray; }
    }

    public int MaxEnemyCount
    {
        get { return maxEnemyCount; }
    }

    public WaveTrigger WaveTriggerRef
    {
        get { return waveTrigger; }
    }
}

[System.Serializable]
public struct SpawnSettings
{
    [SerializeField]
    private EnemyType enemyTypeToSpawn;

    [Range(0, 14)]
    [SerializeField]
    private int spawnLocationIndex;

    [SerializeField]
    private float spawnCooldown;

    public EnemyType EnemyTypeToSpawn
    {
        get { return enemyTypeToSpawn; }
    }

    public int SpawnLocationIndex
    {
        get { return spawnLocationIndex; }
    }

    public float SpawnCooldown
    {
        get { return spawnCooldown; }
    }
}
