using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace XRLab
{
#if UNITY_EDITOR
    [CustomEditor(typeof(BaseCustomTool))]
    public class ProceduralMeshInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ProceduralMeshTool proceduralMeshBuilder = (ProceduralMeshTool)target;

            if (GUILayout.Button("Instance Mesh"))
            {
                //proceduralMeshBuilder.CreateNewMesh();
            }
        }
    }
#endif 
}