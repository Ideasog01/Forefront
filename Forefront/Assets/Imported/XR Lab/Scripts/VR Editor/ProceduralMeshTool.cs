using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace XRLab
{
    [AddComponentMenu("XRLab/Toolgun Tools/Procedural Mesh Tool")]
    public class ProceduralMeshTool : BaseCustomTool
    {
        [System.Serializable]
        protected enum ColliderType
        {
            box = 0,
            sphere = 1,
            capsule = 2,
            mesh = 3,
            none = 4,
        }


        [Header("Collider Settings")]
        [SerializeField] protected ColliderType m_collider;

        [Header("Materials and Shaders")]
        [Tooltip("the material that the mesh will be assigned when it is instanced")]
        [SerializeField] protected Material m_instanceMaterial;
        [Tooltip("the material that will be used while previewing the mesh. if left unassigned, will generate a material")]
        [SerializeField] protected Material m_previewMaterial;
        [Tooltip("the shader the instanced object will have applied to it")]
        [SerializeField] protected Shader m_objectInstanceShader;
        [Tooltip("Should the preview be drawn in wireframe")]
        [SerializeField] private bool m_previewWireframe = false;
        private List<Vector3> m_points = new List<Vector3>();

        protected List<Vector3> Points
        {
            get => m_points;
            set => m_points = value;
        }

        /// <summary>
        /// Called when the primary button is pressed
        /// Overrides parent and adds a point to be added to 
        /// the OpenGL render pool 
        /// </summary>
        /// <param name="context"></param>
        protected override void PrimaryButtonPressed(InputAction.CallbackContext context)
        {
            m_points.Add(gameObject.transform.position);
        }

        /// <summary>
        /// Takes the current points list
        /// and instances an object that will render 
        /// it and allow references from within the editor
        /// </summary>
        /// <param name="context"></param>
        protected override void TriggerPressed(InputAction.CallbackContext context)
        {
            CreateNewMesh();
        }

        /// <summary>
        /// converts the m_points list to a mesh that
        /// does not require openGL to render it directly
        /// </summary>
        protected void CreateNewMesh()
        {
            if (m_points.Count < 2)
                return;

            GameObject obj = new GameObject(); //the game object that will be instantiated
            obj.name = "Procedural Generation Mesh"; //giving the gameobject itself a name

            Mesh mesh = new Mesh(); //the mesh for the object

            //mesh.Clear(); //probably redundant but left here just incase

            mesh.name = "PROCEDURAL_MESH"; //giving the mesh a name

            #region Moving to world centre
            /** 
             * moving the mesh to the world centre to ensure that pivot
             * point is located in the centre of the object. This is
             * nessasary to prevent strange offset which presents itself
             * after adding a grabable component to the object and grabbing it
            **/
            Vector3 centrePivot = FindCentreOfPoints();

            Vector3[] pnts = m_points.ToArray(); //needed to modify elements of point list in a loop

            for (int i = 0; i < pnts.Length; i++)
            {
                pnts[i] -= centrePivot; //subtracting the pivot from each point to have all points in the centre of the world
            }
            #endregion

            mesh.SetVertices(pnts); //setting all the vertices of the mesh

            //generating triangle strip
            int[] tris = new int[(m_points.Count - 2) * 3];
            int currentVertexPoint = 0;

            for (int i = 0; i < tris.Length / 3; i++) //run this loop once per triangle
            {
                for (int j = 0; j < 3; j++)
                {
                    tris[currentVertexPoint] = i + j;
                    currentVertexPoint++;
                }
            }

            mesh.triangles = tris;

            //recalculating mesh information
            mesh.uv = GenerateUVs(); //new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) }; //= GenerateUVs();//
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();

            obj.AddComponent<MeshFilter>().sharedMesh = mesh;

            //if the instance material is assigned then use that
            if (m_instanceMaterial != null)
                obj.AddComponent<MeshRenderer>().material = m_instanceMaterial;
            else //if the instance material isn't assigned, use the preview
                obj.AddComponent<MeshRenderer>().material = m_previewMaterial;

            if (m_objectInstanceShader != null) //if shader is assigned, will override the current shader with the passed one
                obj.GetComponent<MeshRenderer>().material.shader = m_objectInstanceShader;

            switch (m_collider)
            {
                case ColliderType.box:
                    obj.AddComponent<BoxCollider>();
                    break;
                case ColliderType.sphere:
                    obj.AddComponent<SphereCollider>();
                    break;
                case ColliderType.capsule:
                    obj.AddComponent<CapsuleCollider>();
                    break;
                case ColliderType.mesh:
                    obj.AddComponent<MeshCollider>().sharedMesh = mesh;
                    obj.GetComponent<MeshCollider>().convex = true;
                    break;
                case ColliderType.none:
                    break;
                default:
                    Debug.LogError("Attempted to add collider to procedural mesh with null collider type");
                    break;
            }

            m_points.Clear(); //clearing the list so it can be reused

            obj.transform.position = centrePivot; //moving the mesh back to where it was so user can't see it moved
        }

        /// <summary>
        /// generates a centre pivot from vertex data
        /// and moves all vertices by that offset to
        /// bring the mesh to the centre of the world
        /// and returns pivot offset used for transform
        /// </summary>
        /// <returns>Pivot offset applied to vertices</returns>
        protected Vector3 FindCentreOfPoints()
        {
            //holder for the centre pivot
            Vector3 centrePivot = new Vector3(0, 0, 0);

            //adding all of the vectors together
            foreach (Vector3 point in m_points)
            {
                centrePivot += point;
            }

            //averaging the pivot to get the centre point for the vertices
            centrePivot /= m_points.Count;

            return centrePivot;
        }

        /// <summary>
        /// generates UV's for a triangle strip by 
        /// stretching the UV coords evenly along 
        /// the entire strip. This code does not 
        /// compensate for changes in object size 
        /// nor does it take tiling into account 
        /// when generating the uvs
        /// </summary>
        /// <returns>vector2 array with uv coords</returns>
        private Vector2[] GenerateUVs()
        {
            Vector2[] uvs = new Vector2[m_points.Count]; //generating an array of with space for all uvs

            float uvSpacing = 1 / (float)(m_points.Count / 2); //calculating the spacing by calculating an even distance between each point

            for (int i = 0; i < m_points.Count; i++)
            {
                //alternate the X value and set Y value based on the spacing multiplied by the index for even distribution
                if (i % 2 == 0)
                {
                    uvs[i] = new Vector2(1, uvSpacing * i);
                }
                else
                {
                    uvs[i] = new Vector2(0, uvSpacing * i);
                }
            }

            return uvs;
        }
        #region OpenGL
        /// <summary>
        /// after standard rendering is completed,
        /// will run and draw whatever is inside the
        /// m_points list
        /// </summary>
        public void OnRenderObject()
        {
            CreateLineMaterial();

            // Apply the line material
            m_previewMaterial.SetPass(0);
            GLDrawObjectFromList(true, GL.TRIANGLE_STRIP, m_points);
        }

        /// <summary>
        /// a wrapper that will draw a mesh in openGL
        /// using a list of vertexes passed into it. 
        /// </summary>
        /// <param name="drawInWorldSpace"> should the object draw in local or world space coordinate system</param>
        /// <param name="GLDrawMode">the draw mode for openGL to use (triangles, quads, triangle_strip, line_strip, lines)</param>
        /// <param name="points">the list of points to draw</param>
        private void GLDrawObjectFromList(bool drawInWorldSpace, int GLDrawMode, List<Vector3> points)
        {
            GL.PushMatrix();

            if (!drawInWorldSpace)
                GL.MultMatrix(transform.localToWorldMatrix);

            GL.wireframe = m_previewWireframe;

            //setting the draw mode to be used for the current object
            GL.Begin(GLDrawMode);
            //drawing each point currently assigned
            foreach (Vector3 point in points)
            {
                GL.Vertex3(point.x, point.y, point.z);
            }

            GL.End();
            GL.PopMatrix();
        }

        /// <summary>
        /// THIS FUNCTION WAS TAKEN FROM UNITY DOCUMENTATION
        /// Generates a shader that can be applied to objects 
        /// rendered with openGL
        /// </summary>
        private void CreateLineMaterial()
        {
            if (!m_previewMaterial)
            {
                // Unity has a built-in shader that is useful for drawing
                // simple colored things.
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                m_previewMaterial = new Material(shader);
                m_previewMaterial.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                m_previewMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                m_previewMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                m_previewMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // Turn off depth writes
                m_previewMaterial.SetInt("_ZWrite", 0);
            }
        }
        #endregion
    }
}
