using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace XRLab
{
    public class XRLabLib : MonoBehaviour
    {
        #region Detect VR Device
        /// <summary>
        /// different HMD types
        /// </summary>
        public enum VRHMD
        {
            vive,
            vive_pro,
            vive_cosmos,
            rift,
            indexhmd,
            holo_hmd,
            none
        }


        /// <summary>
        /// different controller types
        /// </summary>
        public enum VRController
        {
            vive_controller,
            vive_cosmos_controller,
            oculus_touch,
            knuckles,
            holo_controller,
            none
        }

        public static bool GetHMDConnected()
        {
            return ("OpenXR Display" == XRSettings.loadedDeviceName);
        }
        /// <summary>
        /// Gets the active hmd. Note that in openXR the output will read openXR display
        /// </summary>
        /// <returns></returns>
        public static string GetHMDType()
        {
            return XRSettings.loadedDeviceName;
        }

        /// <summary>
        /// Prints the currently connected display
        /// </summary>
        public static void PrintHMDType()
        {
            Debug.Log("Connected: " + GetHMDConnected() + " type: " + XRSettings.loadedDeviceName);
        }
        #endregion

        /// <summary>
        /// An enumeration representing all 6 directions of movement
        /// </summary>
        [System.Serializable]
        public enum VectorDirections { forward, backward, up, down, left, right }

        /// <summary>
        /// used to store transform data without references
        /// </summary>
        public struct TransformValue { public Vector3 position; public Quaternion rotation; }

        /// <summary>
        /// Takes 2 arrays and returns a new array with the contents
        /// of both array parameters. If types do not match, will produce 
        /// an error  
        /// </summary>
        /// <typeparam name="Template">A generic array type that matches the type of the inputs</typeparam>
        /// <param name="array1">Base array, this will be at the starting index of the array</param>
        /// <param name="array2">Secondary array, this will be after the last index of the base array</param>
        /// <returns></returns>
        public static Template[] JoinArrays<Template>(Template[] array1, Template[] array2)
        {
            Template[] output = new Template[array1.Length + array2.Length];

            array1.CopyTo(output, 0);
            array2.CopyTo(output, array1.Length);

            Debug.Log("outputting an array of type " + output.GetType());
            return output;
        }

        /// <summary>
        /// Returns true if target layer is located 
        /// within layermask, returns false if target
        /// layer is not found
        /// 
        /// Note contains a bug where checking only default layer will always return false
        /// </summary>
        /// <param name="layer">The layer to search for</param>
        /// <param name="layermask">The layer mask to search</param>
        /// <returns>Is in layer mask</returns>
        public static bool IsInLayerMask(int layer, LayerMask layermask)
        {
            return layermask == (layermask | (1 << layer));
        }

        /// <summary>
        /// Quits the program
        /// </summary>
        public static void ExitGame()
        {
            Application.Quit(0);
        }

        /// <summary>
        /// Incomplete function
        /// 
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Vector2 GetAspectRatio(Texture2D input)
        {
            //int aspect (a / gcf(a, b)) * b;
            Debug.Log("INCOMPLETE LIB: GetAspectRatio used but is incomplete, 0,0 will be returned as a placeholder");
            return new Vector2(0, 0);
        }
    }
}

