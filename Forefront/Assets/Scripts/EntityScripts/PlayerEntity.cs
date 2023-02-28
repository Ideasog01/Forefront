using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : BaseEntity
{
    public static bool plasmaActive;

    public static bool laserActive;

    [Header("Plasma Cannon")]

    [SerializeField]
    private Transform plasmaSpawnPos;

    [SerializeField]
    private Transform plasmaProjectilePrefab;

    [Header("Laser")]

    [SerializeField]
    private Transform laserOrigin;

    [SerializeField]
    private LineRenderer laserLineRenderer;

    public void FireCannon()
    {
        if(plasmaActive)
        {
            GameManager.spawnManager.SpawnProjectile(plasmaProjectilePrefab, plasmaSpawnPos.position, plasmaSpawnPos.rotation);
            Debug.Log("Cannon Fired");
        }
    }

    public void FireLaser(bool active)
    {
        if(laserActive)
        {
            if(active)
            {
                //Debug.Log("Laser Started");
            }
            else
            {
                //Debug.Log("Laser Stopped");
            }
        }

        laserLineRenderer.enabled = active;
    }
}
