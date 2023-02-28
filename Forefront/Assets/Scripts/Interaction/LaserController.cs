using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField]
    private LayerMask collisionLayer;

    [SerializeField]
    private Transform lineOrigin;

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

        if(Physics.Raycast(origin, direction, out hit, direction.magnitude, collisionLayer))
        {
            Debug.Log("Laser Collision Detected! Object Hit: " + hit.collider.gameObject.name);

            _lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            _lineRenderer.SetPosition(1, end);
        }
    }
}
