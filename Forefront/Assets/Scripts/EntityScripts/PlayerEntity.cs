using UnityEngine;

public class PlayerEntity : BaseEntity
{
    public static bool plasmaActive;

    public static bool laserActive;

    public static float plasmaCharge;

    public static int scoreAmount;

    public static float powerChargeAmount = 100;

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

    [SerializeField]
    private VisualEffect plasmaChargeEffect;

    [SerializeField]
    private VisualEffect plasmaFireEffect;

    [SerializeField]
    private Sound plasmaChargeSound;

    [SerializeField]
    private Sound plasmaFireSound;

    [Header("Laser")]

    [SerializeField]
    private Transform laserOrigin;

    [SerializeField]
    private LineRenderer laserLineRenderer;

    [SerializeField]
    private float laserChargeCost;

    [SerializeField]
    private Transform laserEnd;

    [SerializeField]
    private GameObject laserEndVFX;

    [SerializeField]
    private float laserRotationSpeed;

    [SerializeField]
    private Sound laserSound;

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

    private void Awake()
    {
        powerChargeAmount = 100;
    }

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

            laserEnd.Rotate(new Vector3(0, 0, 1) * laserRotationSpeed);
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
                    GameManager.visualEffectManager.StartVFX(plasmaFireEffect);
                    GameManager.audioManager.PlaySound(plasmaFireSound);
                    Debug.Log("Cannon Fired");
                }

                plasmaCharge = 0;
                _isPlasmaCharging = false;

                GameManager.guiManager.DisplayPlasmaCharge();
                GameManager.guiManager.TogglePlasmaCharge(false);

                GameManager.visualEffectManager.StopVFX(plasmaChargeEffect);
                GameManager.audioManager.StopSound(plasmaChargeSound);
            }
            else
            {
                plasmaChargeTime = chargeTime[GameManager.mainLoadout.GeneralSettingsValueArray[3]];
                plasmaChargeCost = powerEffectiveness[GameManager.mainLoadout.GeneralSettingsValueArray[2]];

                _isPlasmaCharging = true;
                GameManager.guiManager.TogglePlasmaCharge(true);

                GameManager.visualEffectManager.StartVFX(plasmaChargeEffect);
                GameManager.audioManager.PlaySound(plasmaChargeSound);
            }
        }
    }

    public void FireLaser(bool active)
    {
        if(laserActive)
        {
            _isLaserFiring = active;
            laserLineRenderer.enabled = active;
            
            if(active)
            {
                GameManager.audioManager.PlaySound(laserSound);
            }
            else
            {
                GameManager.audioManager.StopSound(laserSound);
                laserEndVFX.SetActive(false); //As the laser is now disabled, and laser end vfx is activated when the laser collides with an object and deactivated when not.
            }
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
                GameManager.visualEffectManager.StopVFX(plasmaChargeEffect);
                GameManager.audioManager.StopSound(plasmaChargeSound);
            }
        }
        else //Once the charge reaches the charge time, fire a projectile and reset
        {
            FireCannon(true);
        }

        GameManager.guiManager.DisplayPlasmaCharge(); //Continue to display the charge when trigger is pressed and is charging
    }
}
