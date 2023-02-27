using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveTowards : MonoBehaviour
{
    [SerializeField] Transform m_targetTransform;
    [SerializeField] float m_moveSpeed;
    [SerializeField] bool m_lookTowardsTarget = true;
    // Start is called before the first frame update
    void Start()
    {
        if (m_targetTransform == null)
            m_targetTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_targetTransform == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, m_targetTransform.position, m_moveSpeed);

        if(m_lookTowardsTarget)
            transform.LookAt(m_targetTransform, transform.up);
    }
}
