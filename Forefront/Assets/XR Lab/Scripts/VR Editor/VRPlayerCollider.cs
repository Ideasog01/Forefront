using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves a box to the camera position.
/// also adds a rigid body to the player
/// to allow physics to be applied to 
/// someone in VR. 
/// 
/// Note: this script is designed to 
/// be put on the root of an XR Rig 
/// and may not function correctly if 
/// put on other gameobjects
/// </summary>
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class VRPlayerCollider : MonoBehaviour
{
    [SerializeField] Transform m_camTransform;
    [SerializeField] BoxCollider m_boxCollider;
    
    /// <summary>
    /// If references are unassigned, will attempt to automatically
    /// assign the references
    /// </summary>
    void Awake()
    {
        //if the camera transform is unassigned, grab the first camera with the main camera tag
        if (m_camTransform == null)
            m_camTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;

        //if box collider is unassigned, attempt to find it
        if (m_boxCollider == null)
        {
            m_boxCollider = gameObject.GetComponent<BoxCollider>();

            //setting default values
            m_boxCollider.size = new Vector3(1, 0.5f, 1);
            m_boxCollider.center = new Vector3(0, 0.25f, 0);
        }
    }

    /// <summary>
    /// if references are set up correctly, set the centre
    /// position of the box collider to the camera position
    /// while retaining the current Y value
    /// </summary>
    void FixedUpdate()
    {
        //do not run if there are null references
        if (m_camTransform == null || m_boxCollider == null)
            return;

        m_boxCollider.center = new Vector3(m_camTransform.position.x, m_boxCollider.center.y, m_camTransform.position.z);
    }
}
