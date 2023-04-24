using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{

    [SerializeField]
    private Transform lineOrigin;

    [SerializeField]
    private GameObject laserEndVfx;

    [SerializeField]
    private int laserDamage;

    [SerializeField]
    private float damageCooldown;

    private bool _damageInactive;

    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = this.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(_lineRenderer.enabled)
        {
            CheckCollision();
        }
    }

    private void OnEnable()
    {
        _damageInactive = false;
    }

    private void CheckCollision()
    {
        _lineRenderer.SetPosition(0, lineOrigin.position);

        RaycastHit hit;

        Vector3 origin = this.transform.position;
        Vector3 end = this.transform.position + this.transform.forward * 5;
        Vector3 direction = end - origin;

        if(Physics.Raycast(origin, direction, out hit, direction.magnitude))
        {
            Debug.Log("Laser Collision Detected! Object Hit: " + hit.collider.gameObject.name);

            if(hit.collider.CompareTag("Projectile"))
            {
                hit.collider.gameObject.SetActive(false);
            }

            if(!_damageInactive)
            {
                if (hit.collider.CompareTag("EnemyDefault"))
                {
                    hit.transform.parent.GetComponent<EnemyEntity>().TakeDamage(laserDamage);
                }
                else if (hit.collider.CompareTag("EnemyCritical"))
                {
                    hit.transform.parent.GetComponent<EnemyEntity>().TakeDamage(laserDamage * 2);
                }

                StartCoroutine(DelayDamage());
            }

            laserEndVfx.transform.position = hit.point;
            laserEndVfx.SetActive(true);

            _lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            _lineRenderer.SetPosition(1, end);
            laserEndVfx.SetActive(false);
        }
    }

    private IEnumerator DelayDamage()
    {
        _damageInactive = true;

        yield return new WaitForSeconds(damageCooldown);

        _damageInactive = false;
    }
}
