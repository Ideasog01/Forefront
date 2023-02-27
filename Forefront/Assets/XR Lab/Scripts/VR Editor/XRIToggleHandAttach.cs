using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.UI;
/// <summary>
/// Accesses the XRGrabInteractable and attaches the object to 
/// the interactor that picked up the object. The object will 
/// release itself when respective input action is activated
/// </summary>
[RequireComponent(typeof(XRGrabInteractable))]
public class XRIToggleHandAttach : MonoBehaviour
{
    [Header("Release Actions")]
    [Tooltip("The action to release the object on the left hand")]
    [SerializeField] InputActionReference m_leftReleaseAction;
    [Tooltip("The action to release the object on the right hand")]
    [SerializeField] InputActionReference m_rightReleaseAction;
    [Tooltip("A UI button to release the object")]
    [SerializeField] Button m_releaseButtonUI;
    
    [Header("Optional")]
    [Tooltip("The override transform which the object will snap to instead of the hand")]
    [SerializeField] Transform m_overrideTransform;
    
    bool m_attached = false; //when true, will make menu follow the target transform
    bool m_attachLock = false; //used to prevent object attaching again if the user presses the release button

    Transform m_targetTransform; //the transform the object will go to
    Transform m_thisAttachTransform; //a reference to the attach transform of this specific object
    
    XRGrabInteractable m_interactable; //the interactable the script will pull inputs from
    XRBaseInteractor m_lastUsedInteractor; //used for storing a reference to the last interator to activate the script

    public bool attached { get { return m_attached; } }

    // Start is called before the first frame update
    void Start()
    {
        //getting the reference to the interactor
        m_interactable = GetComponent<XRGrabInteractable>();
        //adding a listener so we know when the user picks up the object
        m_interactable.selectExited.AddListener(PickedUp);

        //getting the attach transform if this object
        m_thisAttachTransform = m_interactable.attachTransform;

        //if there isn't a transform then use the current transform instead
        if(m_thisAttachTransform == null)
            m_thisAttachTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (m_attached)
        {
            if (m_overrideTransform != null) //if override is assigned then use that transform
            {
                transform.position = m_overrideTransform.position;
                transform.rotation = m_overrideTransform.rotation * Quaternion.Inverse(m_thisAttachTransform.localRotation); //subtracting the attach rotation offset to the base rotation 
            }
            else //if the override is null then use attach transform
            {
                transform.parent = m_targetTransform;
                transform.localPosition = -m_thisAttachTransform.localPosition; //= m_targetTransform.position + m_thisAttachTransform.localPosition;

                transform.rotation = m_targetTransform.rotation * Quaternion.Inverse(m_thisAttachTransform.localRotation);//correcting the rotations using attach transform
            }

        }

        //logic code for disabling attach code when holding the object
        //note this isn't really required but it fixes bugs that occur 
        //when holding objects that use RT cameras or spawn objects at 
        //specific transform rotations 

        if (m_lastUsedInteractor == null)
            return;

        if (m_lastUsedInteractor.IsSelecting(m_interactable)) //if holding this object
        {
            m_attached = false;
        }
        else //if not holding this object
        {
            if (m_attachLock) //if attachlock is true then we know the player intends to let go of the object
            {
                m_attachLock = false;
            }
        }
    }

    #region Drop and pickup
    /// <summary>
    /// assign the target transform and
    /// set attached to true when the player
    /// releases the select button
    /// 
    /// if the attach lock is true then 
    /// the code will not run. the attachlock 
    /// should only be active if the player 
    /// has pressed the release object button
    /// </summary>
    /// <param name="args"></param>
    void PickedUp(SelectExitEventArgs args)
    {
        if (m_attachLock) //if attach is locked then don't run
            return;

        m_lastUsedInteractor = (XRBaseInteractor)args.interactorObject;
        m_targetTransform = args.interactorObject.GetAttachTransform(m_interactable);
        m_attached = true;
    }

    /// <summary>
    /// Attempts to match the callback button to the corresponding hand
    /// by using the hand name. Note that this code assumes the user has not
    /// changed the default naming of the XR Origin or still has "Left" or 
    /// "Right" in the target transform
    /// </summary>
    /// <param name="context">reference to the button action that activated the function</param>
    void DropItem(InputAction.CallbackContext context)
    {
        if (context.action.ToString().Contains("Left") && m_targetTransform.name.Contains("Left"))
        {
            DropItem();
        }
        else if (context.action.ToString().Contains("Right") && m_targetTransform.name.Contains("Right"))
        {
            DropItem();
        }
    }

    /// <summary>
    /// Disconnects the object from the hand that picked it up
    /// </summary>
    void DropItem()
    {
        gameObject.transform.parent = null;
        m_attached = false;
        m_attachLock = true;
    } 
    #endregion

    #region Binding Callbacks
    /// <summary>
    /// When the object is set to active
    /// </summary>
    private void OnEnable()
    {
        if (m_releaseButtonUI != null)
            m_releaseButtonUI.onClick.AddListener(DropItem);

        if (m_leftReleaseAction != null)
            m_leftReleaseAction.action.started += DropItem;
        
        if (m_rightReleaseAction != null) 
            m_rightReleaseAction.action.started += DropItem;
    }

    /// <summary>
    /// When the object is set to inactive
    /// </summary>
    private void OnDisable()
    {
        if (m_releaseButtonUI != null)
            m_releaseButtonUI.onClick.RemoveAllListeners();

        if (m_leftReleaseAction != null)
            m_leftReleaseAction.action.started -= DropItem;

        if (m_rightReleaseAction != null) 
            m_rightReleaseAction.action.started -= DropItem;
    } 
    #endregion
}
