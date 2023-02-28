using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private Transform projectileParent;

    private List<ProjectileController> _activeProjectiles = new List<ProjectileController>();

    public void SpawnProjectile(Transform prefab, Vector3 position, Quaternion rotation)
    {
        ProjectileController projectileToUse = null;

        foreach(ProjectileController projectile in _activeProjectiles) //Reuse projectile (avoids uneccessary use of memory)
        {
            if(!projectile.gameObject.activeSelf)
            {
                ProjectileController.ProjectileType projectileType = prefab.GetComponent<ProjectileController>().ProjectileTypeRef;

                if(projectileType == projectile.ProjectileTypeRef) //Type matches
                {
                    projectile.gameObject.SetActive(true);
                    projectileToUse = projectile;
                    break;
                }
            }
        }

        if(projectileToUse == null) //Projectile not found, create a new one
        {
            projectileToUse = Instantiate(prefab.GetComponent<ProjectileController>(), position, rotation);
            projectileToUse.transform.parent = projectileParent;
            _activeProjectiles.Add(projectileToUse);
        }

        projectileToUse.InitialiseProjectile(position, rotation); //Reset the projectile
    }

}
