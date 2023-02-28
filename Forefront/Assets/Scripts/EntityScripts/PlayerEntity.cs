using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : BaseEntity
{
    public static bool plasmaActive;

    public static bool laserActive;

    public static float plasmaCharge;

    [Header("Plasma Cannon")]

    [SerializeField]
    private Transform plasmaSpawnPos;

    [SerializeField]
    private Transform[] plasmaProjectilePrefab;

    [SerializeField]
    private float plasmaChargeRate = 30;

    [Header("Laser")]

    [SerializeField]
    private Transform laserOrigin;

    [SerializeField]
    private LineRenderer laserLineRenderer;

    private bool _isPlasmaCharging;

    private void Update()
    {
        if(_isPlasmaCharging)
        {
            if(plasmaCharge < 100) //Increment the plasma charge to 100
            {
                plasmaCharge += Time.deltaTime * plasmaChargeRate;
            }
            else //Once the charge reaches 100, fire a projectile and reset
            {
                FireCannon(true);
            }

            GameManager.guiManager.DisplayPlasmaCharge(); //Continue to display the charge when trigger is pressed and is charging
        }
    }

    public void FireCannon(bool release)
    {
        if(plasmaActive)
        {
            if(release)
            {
                if(plasmaCharge > 0)
                {
                    Transform prefab = null;

                    if(plasmaCharge < 50)
                    {
                        prefab = plasmaProjectilePrefab[0];
                    }
                    else if(plasmaCharge < 90)
                    {
                        prefab = plasmaProjectilePrefab[1];
                    }
                    else
                    {
                        prefab = plasmaProjectilePrefab[2];
                    }   

                    GameManager.spawnManager.SpawnProjectile(prefab, plasmaSpawnPos.position, plasmaSpawnPos.rotation);
                    Debug.Log("Cannon Fired");
                    plasmaCharge = 0;
                    _isPlasmaCharging = false;

                    GameManager.guiManager.DisplayPlasmaCharge();
                    GameManager.guiManager.TogglePlasmaCharge(false);
                }
            }
            else
            {
                _isPlasmaCharging = true;
                GameManager.guiManager.TogglePlasmaCharge(true);
            }
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
