using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int waveIndex; //The current wave the player is on (Starts at wave 0)

    public int spawnIndex; //The current spawn setting

    [SerializeField]
    private WaveDetails[] waveArray;

    [SerializeField]
    private float startDistanceThreshold;

    [SerializeField]
    private bool startWave;

    private bool _enemiesSpawned;

    private bool _playerIsNear;

    private void Start()
    {
        if(startWave)
        {
            StartWave();
        }
    }

    private void Update()
    {
        //If all enemies have been spawned this wave, and all enemies have been defeated, the wave is complete!
        if(_enemiesSpawned)
        {
            if(SpawnManager.activeHostiles == 0)
            {
                _enemiesSpawned = false;
                WaveComplete();
            }
        }

        if(!_playerIsNear)
        {
            float distanceToStartLocation = Vector3.Distance(GameManager.playerEntity.transform.position, waveArray[waveIndex].EncounterTrigger.position);

            if (distanceToStartLocation < startDistanceThreshold)
            {
                _playerIsNear = true;
                BeginEncounter();
                Debug.Log("New Wave Started!");
                waveArray[waveIndex].EncounterTrigger.gameObject.SetActive(false);
            }
        }
    }

    public void BeginEncounter()
    {
        StartCoroutine(DelaySpawn());
    }

    private void StartWave()
    {
        Debug.Log("New Wave Started");
        _playerIsNear = false;
    }

    private void WaveComplete()
    {
        waveIndex++;
        spawnIndex = 0;
        
        if(waveIndex >= waveArray.Length)
        {
            EndGame();
        }
        else
        {
            StartWave();
        }
    }

    private void EndGame()
    {
        Debug.Log("Game Ended");
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
