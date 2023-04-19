using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class WaveManager : MonoBehaviour
{
    public static int missionFails; //static variable as seen is reset on player defeat. (Needs to be reset on return to main menu)

    public int waveIndex; //The current wave the player is on (Starts at wave 0)

    public int spawnIndex; //The current enemy being spawned

    public int playerScore;

    public int hostilesDefeated;

    [SerializeField]
    private WaveDetails[] waveArray;

    private bool _enemiesSpawned;

    private bool _encounterInProgress;

    public void BeginEncounter()
    {
        if(!_encounterInProgress)
        {
            spawnIndex = 0;
            _encounterInProgress = true;
            StartCoroutine(DelaySpawn());
        }
    }

    private void Update()
    {
        //If all enemies have been spawned this wave, and all enemies have been defeated, the wave is complete!
        if(_enemiesSpawned)
        {
            if(SpawnManager.activeHostiles == 0)
            {
                if(waveIndex == waveArray.Length - 1)
                {
                    _encounterInProgress = false;
                    Debug.Log("Wave Complete!");
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

        GameManager.guiManager.DisplayVictoryCanvas();
        Debug.Log("GameMode Ended");
    }

    private IEnumerator DelaySpawn()
    {
        WaveDetails currentWave = waveArray[waveIndex];

        yield return new WaitForSeconds(currentWave.SpawnCooldownArray[spawnIndex]); //Wait for the cooldown

        yield return new WaitUntil(() => SpawnManager.activeHostiles < currentWave.MaxEnemyCount); //Prevents too many enemies from being active

        GameManager.spawnManager.SpawnEnemy(currentWave.EnemyPrefabArray[spawnIndex], currentWave.EnemyTypeArray[spawnIndex], currentWave.SpawnLocationArray[spawnIndex].position, Quaternion.identity);


        Debug.Log("Enemy Spawned!");

        if(spawnIndex < currentWave.SpawnCooldownArray.Length - 1) //Is NOT the end of the wave
        {
            spawnIndex++;
            StartCoroutine(DelaySpawn());
        }
        else
        {
            _enemiesSpawned = true; //Allows for the game to wait until all enemies have been defeated
        }
    }
}

[System.Serializable]
public struct WaveDetails
{
    [SerializeField]
    private Transform[] spawnLocationArray;

    [SerializeField]
    private Transform[] enemyPrefabArray;

    [SerializeField]
    private EnemyEntity.EnemyType[] enemyTypeArray;

    [SerializeField]
    private float[] spawnCooldownArray;

    [SerializeField]
    private int maxEnemyCount;

    [SerializeField]
    private Transform encounterTrigger;

    public Transform[] SpawnLocationArray
    {
        get { return spawnLocationArray; }
    }

    public Transform[] EnemyPrefabArray
    {
        get { return enemyPrefabArray; }
    }

    public EnemyEntity.EnemyType[] EnemyTypeArray
    {
        get { return enemyTypeArray; }
    }

    public float[] SpawnCooldownArray
    {
        get { return spawnCooldownArray; }
    }

    public int MaxEnemyCount
    {
        get { return maxEnemyCount; }
    }

    public Transform EncounterTrigger
    {
        get { return encounterTrigger; }
    }
}
