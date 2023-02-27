using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab
{
    /// <summary>
    /// Derives from SimpleTarget
    /// 
    /// checks for a collision and when one is detected by the base class
    /// will add an explosion force to all child objects of the gameObject
    /// and reset the positions after m_resetTimeSeconds has passed
    /// </summary>
    public class ExplodingTarget : SimpleTarget
    {
        [Header("Explosion Variables")]
        [Tooltip("The radius of the explosion force applied to all objects")]
        [SerializeField] private float m_explosionRadius = 5f;
        [Tooltip("The power of the explosive force that affects all objects")]
        [SerializeField] private float m_explosionPower = 10f;
        [Tooltip("The centre origin point of the explosion. If unassigned, m_hitpos from base class will be used")]
        [SerializeField] private Transform m_explosionCentre;

        private GameObject[] m_objects; //a reference to the objects that will be fired then hit
        private XRLabLib.TransformValue[] m_resetPositions; //the transforms the target spawns at

        protected override void Start()
        {
            base.Start();

            Transform[] m_startingTransforms = gameObject.GetComponentsInChildren<Transform>(); //used to reference all gameObjects

            m_objects = new GameObject[m_startingTransforms.Length]; //set array to size of transforms array
            m_resetPositions = new XRLabLib.TransformValue[m_startingTransforms.Length]; //set array to size of transforms array

            //initialise object and reset arrays 
            for (int i = 0; i < m_objects.Length; i++)
            {
                m_objects[i] = m_startingTransforms[i].gameObject;
                m_resetPositions[i].position = m_objects[i].transform.position;
                m_resetPositions[i].rotation = m_objects[i].transform.rotation;
            }
        }

        /// <summary>
        /// Applies an explosion force to all objects in 
        /// m_objects then waits for m_resetTimeSeconds
        /// and resets all objects back to their starting 
        /// positions
        /// </summary>
        /// <returns>Wait time before reset</returns>
        protected override IEnumerator TargetHit()
        {
            m_collider.enabled = false; //setting collider to false to prevent additional inputs

            if (m_objects != null) //prevent code running if objects are null
            {
                for (int i = 1; i < m_objects.Length; i++) //adding the explosion force to all objects
                {
                    if (m_explosionCentre != null) //if a explosion centre is assigned use that
                        m_objects[i].GetComponent<Rigidbody>().AddExplosionForce(m_explosionPower, m_explosionCentre.position, m_explosionRadius * Random.Range(1, 5), 0f);
                    else //if no explosion centre is assigned then use the hitPos from the base class
                        m_objects[i].GetComponent<Rigidbody>().AddExplosionForce(m_explosionPower, m_hitPos, m_explosionRadius * Random.Range(1, 5), 0f);
                }

                yield return new WaitForSeconds(m_resetTimeSeconds); //waiting until it's time to reset the positions

                for (int i = 1; i < m_objects.Length; i++) //starting from 1 to ignore root object
                {
                    //zero out the velocity and angular velocity so they don't move when we reset them
                    m_objects[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                    m_objects[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                    //reset the transform
                    m_objects[i].transform.SetPositionAndRotation(m_resetPositions[i].position, m_resetPositions[i].rotation);
                }
            }

            m_collider.enabled = true;
        }
    } 
}
