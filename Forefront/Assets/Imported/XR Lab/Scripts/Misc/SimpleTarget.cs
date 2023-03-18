using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab
{
    /// <summary>
    /// A simple base class for a target that does something 
    /// when intersected by an object with a collider
    /// </summary>
    [AddComponentMenu("XRLab/General/Simple Target")]
    public class SimpleTarget : MonoBehaviour
    {
        [SerializeField] protected Collider m_collider;

        [Header("Base Target Parameters")]
        [Tooltip("Time until the target should be reset back to normal")]
        [SerializeField] protected float m_resetTimeSeconds = 5f;

        [Tooltip("A mask for objects that should be destroyed when they hit the target")]
        [SerializeField] private LayerMask m_destroyObjectsLayerMask;

        private MeshRenderer m_meshRenderer; 
        protected Vector3 m_hitPos; //used to keep track of the contact position of intersecting objects

        protected virtual void Start()
        {
            if (m_meshRenderer == null)
                m_meshRenderer = GetComponent<MeshRenderer>();

            if(m_collider == null)
                m_collider = GetComponent<Collider>();
        }

        /// <summary>
        /// If there is a collider on the object, this will 
        /// trigger when an object with a collider intersects.
        /// 
        /// Destroys the intersecting object and runs the
        /// TargetHit function for additional functionality
        /// </summary>
        /// <param name="collision">The collision data from the intersecting object</param>

        private void OnCollisionEnter(Collision collision)
        {
            m_hitPos = collision.transform.position;
            //only destroy the object if it is part of the target layer mask
            if (XRLabLib.IsInLayerMask(collision.gameObject.layer, m_destroyObjectsLayerMask))
            {
                //destroying the intersecting object
                Destroy(collision.gameObject);
            }

            //running the hit function
            StartCoroutine(TargetHit());
        }

        protected virtual IEnumerator TargetHit()
        {
            if (m_meshRenderer == null) //if mesh renderer is null
            {
                Debug.LogError("ERROR: no mesh filter on gameobject");
            }
            else //if there is a mesh renderer
            {

                m_meshRenderer.enabled = false;

                yield return new WaitForSeconds(m_resetTimeSeconds);

                m_meshRenderer.enabled = true;
            }
        }
    }
}
