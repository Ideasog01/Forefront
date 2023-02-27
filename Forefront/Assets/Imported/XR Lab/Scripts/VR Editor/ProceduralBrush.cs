using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

namespace XRLab
{
    [AddComponentMenu("XRLab/Toolgun Tools/Procedural Brush Tool")]
    public class ProceduralBrush : ProceduralMeshTool
    {
        private bool m_generatingMesh = false;

        bool m_pointGenerationSide = true; //used solely for deciding which offset side to generate a point on when making strips

        [Header("Brush Settings")]
        [SerializeField] private float m_meshPointSpacing = 0.05f;
        [SerializeField] private float m_brushWidth = 0.05f;
        [SerializeField] private bool m_hullMode = false;
        /// <summary>
        /// if m_generatingMesh is true then will add points to a
        /// list to be used by openGL for rendering a mesh
        /// </summary>
        void FixedUpdate()
        {
            if (!m_generatingMesh)
                return;

            if (Vector3.Distance(gameObject.transform.position, base.Points.Last()) > m_meshPointSpacing)
            {
                m_pointGenerationSide = !m_pointGenerationSide; //toggle the generation side

                if (m_pointGenerationSide) //right side
                    base.Points.Add(gameObject.transform.TransformPoint(m_brushWidth, 0, 0));
                else //left side
                    base.Points.Add(gameObject.transform.TransformPoint(-m_brushWidth, 0, 0));

                if (m_hullMode)
                    base.Points.Add(base.Points.First());
            }
        }

        /// <summary>
        /// adds a single point and activates the m_generatingMesh bool
        /// so more points will be continually added as the player moves
        /// </summary>
        /// <param name="context"></param>
        protected override void TriggerPressed(InputAction.CallbackContext context)
        {
            //adding first point so continual mesh has something to initially reference
            base.Points.Add(gameObject.transform.position);
            //set generating mesh to true so the update cycle starts running
            m_generatingMesh = true;
        }

        /// <summary>
        /// when the player releases the trigger, stop new points from 
        /// being generated and then instance the mesh using the parent 
        /// class
        /// </summary>
        /// <param name="context"></param>
        protected override void TriggerReleased(InputAction.CallbackContext context)
        {
            //setting generate mesh to false so it stops drawing points 
            m_generatingMesh = false;

            //instancing the mesh using the parent class
            base.CreateNewMesh();
        }

        /// <summary>
        /// function only required to prevent input on
        /// parent class. new functionality could be 
        /// added here if required
        /// </summary>
        /// <param name="context"></param>
        protected override void PrimaryButtonPressed(InputAction.CallbackContext context)
        {
            //only need this to stop functionality from parent class
            //base.PrimaryButtonPressed(context);
        }
    } 
}