using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    public static int activeHostiles;

    [SerializeField]
    private Transform projectileParent;

    [SerializeField]
    private Transform enemyParent;

    [SerializeField]
    private bool activateGameMode;

    private List<EnemyEntity> _enemyList = new List<EnemyEntity>();

    private List<PlayerProjectileController> _activePlayerProjectiles = new List<PlayerProjectileController>();

    private void Start()
    {
        if(activateGameMode)
        {
            SpawnWaveManager();
        }
    }

    public void SpawnWaveManager()
    {
        GameManager.waveManager = Instantiate(GameManager.gameSettings.SpawnPrefab.GetComponent<WaveManager>(), Vector3.zero, Quaternion.identity); //Spawn the WaveManager dependent on the selected game settings (difficulty level)
    }

    public void SpawnPlayerProjectile(Transform prefab, Vector3 position, Quaternion rotation)
    {
        PlayerProjectileController projectileToUse = null;

        foreach (PlayerProjectileController projectile in _activePlayerProjectiles) //Reuse projectile (avoids uneccessary use of memory)
        {
            if (!projectile.gameObject.activeSelf)
            {
                projectile.gameObject.SetActive(true);
                projectileToUse = projectile;
                break;
            }
        }

        if (projectileToUse == null) //Projectile not found, create a new one
        {
            projectileToUse = Instantiate(prefab.GetComponent<PlayerProjectileController>(), position, rotation);
            projectileToUse.transform.parent = projectileParent;
            _activePlayerProjectiles.Add(projectileToUse);
        }

        projectileToUse.InitialiseProjectile(position, rotation); //Reset the projectile
    }

    public void SpawnEnemy(Transform prefab, EnemyEntity.EnemyType type, Vector3 position, Quaternion rotation)
    {
        EnemyEntity enemyEntity = null;

        foreach(EnemyEntity enemy in _enemyList)
        {
            if(!enemy.gameObject.activeSelf)
            {
                if(enemy.EnemyTypeRef == type)
                {
                    enemy.gameObject.SetActive(true);
                    enemyEntity = enemy;
                    break;
                }
            }
        }

        if(enemyEntity == null)
        {
            enemyEntity = Instantiate(prefab.GetComponent<EnemyEntity>(), position, rotation);
            enemyEntity.transform.parent = enemyParent;
            _enemyList.Add(enemyEntity);
        }

        enemyEntity.ResetEnemy();

        activeHostiles++;
    }

}
