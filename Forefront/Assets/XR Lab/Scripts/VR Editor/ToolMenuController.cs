using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRLab
{
    [AddComponentMenu("XRLab/XR Editor/Tool Menu Controller")]
    [RequireComponent(typeof(XRGrabInteractable))]
    public class ToolMenuController : MonoBehaviour
    {
        [System.Serializable]
        private enum TargetMenuType
        {
            Material, Rigidbody, Component, Spawn
        }

        [System.Serializable]
        private struct MenuSummon
        {
            public TargetMenuType Type;
            public Button button;
            public GameObject menu;
            public Transform targetPosition;
        }

        [System.Serializable]
        private struct ToolSwitch
        {
            public Button button;
            public MonoBehaviour targetScript;
        }

        XRGrabInteractable m_InteractableBase; //used to reference the interactor that activates this script
        XRIToggleHandAttach m_toggleHandAttach;

        [Header("Menu Summon")]
        [SerializeField] MenuSummon[] m_menuSummonButtons;
        [SerializeField] private Transform m_toolMenu;
        [SerializeField] private InputActionReference m_xAxisInput;
        [SerializeField] private float m_rotationStep = 5f;

        bool m_enableRan = false; //used to prevent re-assignment of summon buttons 
        private Vector2 m_inputPos;
        private bool m_isRotatingMenu = false;
        private int m_targetMenuRotation = 0;

        [Header("Toolgun Switching")]
        [Tooltip("holds buttons and the components you wish to activate")]
        [SerializeField] ToolSwitch[] m_toolSwitch;
        [Tooltip("Reference to the toolgun controller that handles which tool is active, if unassigned will attempt to find reference automatically")]
        [SerializeField] private XRIToolgunController m_toolgunController;

        [Header("Object Selector")]
        [Tooltip("If true, add components for object selector on pickup")]
        [SerializeField] bool m_addObjectSelector = false;

        [Tooltip("The control that the script object selector should listen for")]
        [SerializeField] private InputActionReference m_selectObjectControl;

        private XRIObjectSelector m_objectSelector; //reference to object selector used to remove or disable component when menu is dropped
        private XRInteractorLineVisual m_interactorLineVisual; //reference to the interactor line visual used to remove/disable line visual when menu is dropped

        [Header("Simulated Settings")]
        [SerializeField] private InputAction m_simXAxisLeft;
        [SerializeField] private InputAction m_simXAxisRight;
        [SerializeField] private InputAction m_simSelectObjectControl;

        private void Start()
        {
            m_toggleHandAttach = gameObject.GetComponent<XRIToggleHandAttach>();

            //attempting to find a reference if left unassigned
            if (m_toolgunController == null)
                m_toolgunController = GameObject.FindObjectOfType<XRIToolgunController>();

            //simulated setups
            //binding actions for rotating the menuy
            m_simXAxisLeft.started += SimRotateLeft;
            m_simXAxisRight.started += SimRotateRight;
            //enabling the bindings so they get tracked
            m_simXAxisLeft.Enable();
            m_simXAxisRight.Enable();


            //adding the simulated controls into the stock input action
            foreach (InputBinding binding in m_simSelectObjectControl.bindings)
            {
                m_selectObjectControl.action.AddBinding(binding.path);
            }
        }

        #region Rotate Menu
        // Update is called once per frame
        void Update()
        {
            //if the value is 360 then set to zero as rotations cycle to correct rotation on next frame
            if (m_targetMenuRotation == 360)
                m_targetMenuRotation = 0;

            if ((int)m_toolMenu.localEulerAngles.y == (int)m_targetMenuRotation)
            {
                m_isRotatingMenu = false;
            }
            else
            {
                m_toolMenu.localRotation = Quaternion.RotateTowards(m_toolMenu.localRotation, Quaternion.Euler(0, m_targetMenuRotation, 0), m_rotationStep);
            }
        }

        private void RotateMenu(InputAction.CallbackContext context)
        {
            Debug.Log("Active Rotation XR");
            if (m_toggleHandAttach != null) //chacking if the toggle hand attach exists
            {
                if (!m_toggleHandAttach.attached) //if the menu is not attached return to disable rotate code
                {
                    return;
                }
            }

            if (!m_isRotatingMenu) //only update the target rotation for 1 input so it snaps to a specific point
            {
                m_inputPos = context.ReadValue<Vector2>();

                if (m_inputPos.x > 0)
                {
                    m_targetMenuRotation = (int)m_toolMenu.localEulerAngles.y - 120;

                    //modulus might be better but this works for now
                    if (m_targetMenuRotation < 0) //forcing the rotation to always be a positive value
                    {
                        m_targetMenuRotation += 360;// - m_targetMenuRotation;
                    }
                }
                else if (m_inputPos.x < 0)
                {
                    m_targetMenuRotation = (int)m_toolMenu.localEulerAngles.y + 120;
                }

                m_isRotatingMenu = true;
            }
        }

        private void RotateMenu(bool direction)
        {
            Debug.Log("Active Rotation Simulated");

            if (m_toggleHandAttach != null) //chacking if the toggle hand attach exists
            {
                if (!m_toggleHandAttach.attached) //if the menu is not attached return to disable rotate code
                {
                    return;
                }
            }

            if (!m_isRotatingMenu) //only update the target rotation for 1 input so it snaps to a specific point
            {
                if (direction)
                {
                    m_targetMenuRotation = (int)m_toolMenu.localEulerAngles.y - 120;

                    if (m_targetMenuRotation < 0) //forcing the rotation to always be a positive value
                    {
                        m_targetMenuRotation += 360;
                    }
                }
                else
                {
                    m_targetMenuRotation = (int)m_toolMenu.localEulerAngles.y + 120;
                }

                m_isRotatingMenu = true;
            }
        }

        private void SimRotateLeft(InputAction.CallbackContext context)
        {
            Debug.Log("ping L");
            RotateMenu(true);
        }

        private void SimRotateRight(InputAction.CallbackContext context)
        {
            Debug.Log("ping R");
            RotateMenu(false);
        }
        #endregion

        private void SummonMenu(GameObject menu, Transform summonPos)
        {
            Debug.Log("TMC attached to " + gameObject.name + " ran summon");

            if (summonPos == null)
                summonPos = gameObject.transform;

            menu.SetActive(true); //showing the menu if it was hidden
            menu.transform.SetPositionAndRotation(summonPos.position, summonPos.rotation); //moving the menu to the target location
        }

        /// <summary>
        /// Is triggered when the lelect 
        /// </summary>
        /// <param name="args"></param>
        void PickedUp(SelectEnterEventArgs args)
        {
            m_InteractableBase.colliders[0].enabled = false;

            if (m_addObjectSelector && m_objectSelector == null) //if true, add components required for selecting objects
            {
                m_objectSelector = gameObject.AddComponent<XRIObjectSelector>(); //----------------------------------------------------------------------------------------------------------------------------------------------
                m_objectSelector.SetControl(m_selectObjectControl);
                m_interactorLineVisual = gameObject.AddComponent<XRInteractorLineVisual>();
            }
        }

        void Dropped(SelectExitEventArgs args)
        {
            //m_InteractableBase.gameObject.SetActive(false);

            if (m_addObjectSelector) //if true, delete components from the object selector
            {
                XRRayInteractor ray = gameObject.GetComponent<XRRayInteractor>();
                LineRenderer lr = gameObject.GetComponent<LineRenderer>();


                if (m_objectSelector != null)
                    Destroy(m_objectSelector);

                if (m_interactorLineVisual != null)
                    Destroy(m_interactorLineVisual);

                //these checks could possibly be removed as they should be automatically added by the previous 2 components
                if (ray != null)
                    Destroy(ray);

                if (lr != null)
                    Destroy(lr);

                Debug.Log("Removed objectSelector and interactor line visual");
            }
        }

        /// <summary>
        /// Enables callback capture for input actions and binds 
        /// 
        /// </summary>
        private void OnEnable()
        {
            m_InteractableBase = GetComponent<XRGrabInteractable>();

            //set up listeners
            m_InteractableBase.selectEntered.AddListener(PickedUp);
            m_InteractableBase.selectExited.AddListener(Dropped);

            if (m_enableRan)
                return;

            for (int ms = 0; ms < m_menuSummonButtons.Length; ms++)
            {
                //if there is a null reference then get the target type and assign it
                if (m_menuSummonButtons[ms].menu == null)
                {
                    switch (m_menuSummonButtons[ms].Type)
                    {
                        case TargetMenuType.Material:
                            m_menuSummonButtons[ms].menu = GameObject.FindObjectOfType<ObjectMaterialManager>().gameObject;
                            break;
                        case TargetMenuType.Rigidbody:
                            m_menuSummonButtons[ms].menu = GameObject.FindObjectOfType<ObjectRigidBodyManager>().gameObject;
                            break;
                        case TargetMenuType.Component:
                            m_menuSummonButtons[ms].menu = GameObject.FindObjectOfType<XRIObjectComponentManager>().gameObject;
                            break;
                        case TargetMenuType.Spawn:
                            m_menuSummonButtons[ms].menu = GameObject.FindObjectOfType<ObjectSpawner>().gameObject;
                            break;
                        default:
                            break;
                    }
                }
            }

            //for some reason this code always gets a null reference in the above loop
            //even if it gets hard coded for index 0 of the array
            foreach (MenuSummon ms in m_menuSummonButtons)
            {
                if (ms.button != null)
                    ms.button.onClick.AddListener(() => SummonMenu(ms.menu, ms.targetPosition));
            }

            m_xAxisInput.action.started += RotateMenu;

            foreach (ToolSwitch ts in m_toolSwitch)
            {
                if (ts.button != null)
                    ts.button.onClick.AddListener(() => m_toolgunController.CurrentCustomTool = ts.targetScript);
            }

            m_enableRan = true;
            Debug.Log("completed enable");
        }

        /// <summary>
        /// disables all button listeners in the 
        /// m_menuSummonButtons and m_toolSwitch
        ///arrays respectively
        /// </summary>
        private void OnDisable()
        {
            foreach (MenuSummon ms in m_menuSummonButtons)
            {
                ms.button.onClick.RemoveAllListeners();
            }

            m_xAxisInput.action.started -= RotateMenu;

            foreach (ToolSwitch ts in m_toolSwitch)
            {
                ts.button.onClick.RemoveAllListeners();
            }
            Debug.Log("completed enable");
        }
    }
}