using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    private float projectileMovementSpeed;

    [SerializeField]
    private int projectileDamage;

    [SerializeField]
    private ProjectileType projectileType;

    [SerializeField]
    private float projectileDuration;

    [SerializeField]
    private VisualEffect destroyVfx;

    [SerializeField]
    private Sound destroySound;

    public enum ProjectileType { PlasmaProjectileSmall, PlasmaProjectileMedium, PlasmaProjectileLarge, DroneProjectile };

    public ProjectileType ProjectileTypeRef
    {
        get { return projectileType; }
    }

    public int ProjectileDamage
    {
        get { return projectileDamage; }
    }

    public void InitialiseProjectile(Vector3 position, Quaternion rotation) //Reset projectile values
    {
        this.transform.position = position;
        this.transform.rotation = rotation;
        StartCoroutine(DelayDisable());
    }

    private void Update()
    {
        ProjectileMovement();
    }

    private void ProjectileMovement()
    {
        this.transform.position += this.transform.forward * Time.deltaTime * projectileMovementSpeed;
    }

    private IEnumerator DelayDisable()
    {
        yield return new WaitForSeconds(projectileDuration);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            GameManager.visualEffectManager.StartVFX(destroyVfx);
            GameManager.audioManager.PlaySound(destroySound);
            this.gameObject.SetActive(false);
        }
    }
}
