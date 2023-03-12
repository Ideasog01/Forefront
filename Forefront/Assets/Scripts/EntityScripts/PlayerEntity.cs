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
    private Transform plasmaProjectilePrefab;

    [SerializeField]
    private float plasmaChargeRate;

    [SerializeField]
    private float plasmaChargeCost;

    [SerializeField]
    private float plasmaChargeTime;

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

    #region HandCannonSettings

    [Header("Hand Cannon Settings")]

    [SerializeField]
    private float[] projectileBlastRadius;

    [SerializeField]
    private float[] projectileVelocity;

    [SerializeField]
    private float[] powerEffectiveness;

    [SerializeField]
    private float[] chargeTime;

    [SerializeField]
    private int[] damageOutput;

    public float[] ProjectileBlastRadius
    {
        get { return projectileBlastRadius; }
    }

    public float[] ProjectileVelocity
    {
        get { return projectileVelocity; }
    }

    public int[] DamageOutput
    {
        get { return damageOutput; }
    }

    public float PlasmaChargeTime
    {
        get { return plasmaChargeTime; }
    }

    #endregion

    private void Update()
    {
        if(_isPlasmaCharging)
        {
            ChargePlasmaCannon();
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
                if(plasmaCharge >= plasmaChargeTime)
                {
                    GameManager.spawnManager.SpawnPlayerProjectile(plasmaProjectilePrefab, plasmaSpawnPos.position, plasmaSpawnPos.rotation);
                    Debug.Log("Cannon Fired");
                }

                plasmaCharge = 0;
                _isPlasmaCharging = false;

                GameManager.guiManager.DisplayPlasmaCharge();
                GameManager.guiManager.TogglePlasmaCharge(false);
            }
            else
            {
                plasmaChargeTime = chargeTime[GameManager.mainLoadout.GeneralSettingsValueArray[3]];
                plasmaChargeCost = powerEffectiveness[GameManager.mainLoadout.GeneralSettingsValueArray[2]];

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

    private void ChargePlasmaCannon()
    {
        if (plasmaCharge < plasmaChargeTime) //Increment the plasma charge to the charge time
        {
            plasmaCharge += Time.deltaTime * plasmaChargeRate;
            powerChargeAmount -= Time.deltaTime * plasmaChargeCost;

            if (powerChargeAmount <= 0)
            {
                Debug.Log("Cannon ran out of charge");
                plasmaCharge = 0;
                _isPlasmaCharging = false;

                GameManager.guiManager.DisplayPlasmaCharge();
                GameManager.guiManager.TogglePlasmaCharge(false);
            }
        }
        else //Once the charge reaches the charge time, fire a projectile and reset
        {
            FireCannon(true);
        }

        GameManager.guiManager.DisplayPlasmaCharge(); //Continue to display the charge when trigger is pressed and is charging
    }
}
