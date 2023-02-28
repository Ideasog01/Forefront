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

    public enum ProjectileType { PlasmaProjectileSmall, PlasmaProjectileMedium, PlasmaProjectileLarge, DroneProjectile };

    public ProjectileType ProjectileTypeRef
    {
        get { return projectileType; }
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

    private void OnCollisionEnter(Collision collision)
    {
        this.gameObject.SetActive(false);
    }

    private IEnumerator DelayDisable()
    {
        yield return new WaitForSeconds(projectileDuration);
        this.gameObject.SetActive(false);
    }
}