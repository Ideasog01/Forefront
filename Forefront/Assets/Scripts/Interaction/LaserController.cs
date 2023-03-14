using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{

    [SerializeField]
    private Transform lineOrigin;

    [SerializeField]
    private GameObject laserEndVfx;

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
}
