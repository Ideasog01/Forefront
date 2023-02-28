using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : BaseEntity
{
    public static bool plasmaActive;

    public static bool laserActive;

    public static float plasmaCharge;

    public static int scoreAmount;

    public static float powerChargeAmount;

    [Header("Plasma Cannon")]

    [SerializeField]
    private Transform plasmaSpawnPos;

    [SerializeField]
    private Transform[] plasmaProjectilePrefab;

    [SerializeField]
    private float plasmaChargeRate = 30;

    [SerializeField]
    private float plasmaChargeCost = 30;

    [Header("Laser")]

    [SerializeField]
    private Transform laserOrigin;

    [SerializeField]
    private LineRenderer laserLineRenderer;

    [SerializeField]
    private float laserChargeCost;

    [Header("General")]

    [SerializeField]
    private float powerRechargeRate;

    private bool _isPlasmaCharging;

    private bool _isLaserFiring;

    private void Update()
    {
        if(_isPlasmaCharging)
        {
            if(plasmaCharge < 100) //Increment the plasma charge to 100
            {
                plasmaCharge += Time.deltaTime * plasmaChargeRate;
                powerChargeAmount -= Time.deltaTime * plasmaChargeCost;

                if(powerChargeAmount <= 0)
                {
                    FireCannon(true);
                }
            }
            else //Once the charge reaches 100, fire a projectile and reset
            {
                FireCannon(true);
            }

            GameManager.guiManager.DisplayPlasmaCharge(); //Continue to display the charge when trigger is pressed and is charging
        }

        if(_isLaserFiring)
        {
            powerChargeAmount -= Time.deltaTime * laserChargeCost;

            if(powerChargeAmount <= 0)
            {
                FireLaser(false);
            }
        }

        if(powerChargeAmount < 100)
        {
            powerChargeAmount += Time.deltaTime * powerRechargeRate;
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
            _isLaserFiring = active;
            laserLineRenderer.enabled = active;
        }
    }
}
