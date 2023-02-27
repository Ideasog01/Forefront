#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using UnityEngine;
using System.Linq;
using System.IO;
namespace XRLab.PackageInstallers
{

    [InitializeOnLoad]
    static class PackageInstaller
    {
        static AddRequest m_packageRequest; //A holder for the requested package
        const string m_openXR = "com.unity.xr.openxr@1.5.3";
        const string m_xrit = "com.unity.xr.interaction.toolkit@2.0.3";
        

        //static PackageInstaller()
        //{
        //    AddOpenXR();
        //    AddXRIT();
        //}

        /// <summary>
        /// Legacy code for checking packages on unity version 2020 and below
        /// </summary>
        /// <param name="pkg">target package name</param>
        /// <returns></returns>
        private static bool HasPackage(string pkg)
        {

            var pack = Client.List();
            //while (!pack.IsCompleted) yield return null;

            var hasPkg = pack.Result.FirstOrDefault(q => q.name == "com.unity.progrids");
            Debug.Log(hasPkg);

            //var pack = Client.List();

            //UnityEditor.PackageManager.PackageInfo pkgHold = null;
            
            //while (!pack.IsCompleted)
            //{
            //    //Debug.Log("" + pack.Result);
            //    pkgHold = pack.Result.FirstOrDefault(q => q.name == pkg);
            //}

            //Debug.Log("Installed " + pkg + ": " + (pkgHold == null));
            
            return (hasPkg == null);
        }

        public static bool IsPackageInstalled(string packageId)
        {
            if (!File.Exists("Packages/manifest.json"))
                return false;

            string jsonText = File.ReadAllText("Packages/manifest.json");
            return jsonText.Contains(packageId);
        }

        #region Package Request menu items
        [MenuItem("XRLab/Package Installers/Install OpenXR")]
        public static void AddOpenXR() //adds the openXR package into the project
        {
            AddPackage(m_openXR, true);

            //if (IsPackageInstalled(m_openXR))
            //{
            //    EditorUtility.DisplayDialog("Install Package", "Package " + m_openXR + " was successfully installed", "Ok");
            //}
            //else
            //{
            //    EditorUtility.DisplayDialog("Install Package", "Package " + m_openXR + " was not installed due to an unknown error", "Ok");
            //}
        }

        [MenuItem("XRLab/Package Installers/Install XR Interaction Toolkit")]
        public static void AddXRIT()
        {
            AddPackage(m_xrit, true);

            //if (IsPackageInstalled(m_xrit))
            //{
            //    EditorUtility.DisplayDialog("Install Package", "Package " + m_openXR + " was successfully installed", "Ok");
            //}
            //else
            //{
            //    EditorUtility.DisplayDialog("Install Package", "Package " + m_openXR + " was not installed due to an unknown error", "Ok");
            //}
        }
        #endregion

        /// <summary>
        /// Checks if the requested package is installed and if it is
        /// not present it will install the package
        /// </summary>
        /// <param name="pkg">The requested package</param>
        /// <param name="notifyUser">Should a confirmation textbox be shown</param>
        private static void AddPackage(string pkg, bool notifyUser)
        {
            if (IsPackageInstalled(pkg))
            {
                Debug.Log("Package " + pkg + " is alredy installed");
                return;
            }
            else if(notifyUser)
            {
                EditorUtility.DisplayDialog("Install Package", "Package " + pkg + " will now be installed", "Ok");
            }

            //the package to install - Note: update the @1.5.3 version when a new version is verified to work correctly
            m_packageRequest = Client.Add(pkg);
            //add a binding to the editor
            EditorApplication.update += InstallPackage;
        }

        /// <summary>
        /// installs the package specified in m_packageRequest
        /// </summary>
        private static void InstallPackage()
        {
            //if the package has finished installing
            if (m_packageRequest.IsCompleted)
            {
                //if the package installed as expected
                if (m_packageRequest.Status == StatusCode.Success)
                {
                    Debug.Log(m_packageRequest.Result.packageId + " was successfully installed within this project");
                    EditorUtility.DisplayDialog("Install Package", "Package " + m_packageRequest.Result.packageId + " was successfully installed", "Ok");
                } 
                //if the package didn't install
                else if (m_packageRequest.Status >= StatusCode.Failure)
                {
                    Debug.LogError(m_packageRequest.Error.message);
                    EditorUtility.DisplayDialog("Install Package Error", m_packageRequest.Error.message, "Ok");
                }
                //unknown errors that shouldn't be possible
                else
                {
                    Debug.LogError("Unknown Critical Error");
                    EditorUtility.DisplayDialog("Install Package Error", "Package " + m_packageRequest.Result.packageId + " was not installed \n unknown critical error", "Ok");

                }

                //clear the binding from the editor
                EditorApplication.update -= InstallPackage;
            }
        }
    }

}
#endif
