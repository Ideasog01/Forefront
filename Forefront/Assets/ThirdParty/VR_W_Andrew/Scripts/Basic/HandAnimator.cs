using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

/// <summary>
/// Passes floating point animation scalers
/// to an animation based on the float
/// values returned by the XR trigger and 
/// grip values
/// </summary>
public class HandAnimator : MonoBehaviour
{
    public enum hand { left, right}

    [SerializeField] private float m_transitionSpeed = 5f;
    [SerializeField] private ActionBasedController m_controller;
    [SerializeField] private hand m_hand;

    private Animator m_animator;
    [SerializeField] private InputAction m_gripAction;
    [SerializeField] private InputAction m_pointAction;

    //List of fingers that should be animated when the grip button is pressed
    private readonly List<Finger> m_gripFingers = new List<Finger>()
    {
        new Finger(Finger.FingerType.Middle),
        new Finger(Finger.FingerType.Ring),
        new Finger(Finger.FingerType.Pinky)
    };

    //List of fingers that should be animated then the trigger is pressed
    private readonly List<Finger> m_pointFingers = new List<Finger>()
    {
        new Finger(Finger.FingerType.Index),
        new Finger(Finger.FingerType.Thumb)
    };

    /// <summary>
    /// Attempts to get the animator that is
    /// attached to the same object as this script
    /// </summary>
    private void Awake()
    {
        if (m_hand == hand.left)
        {
            m_pointAction.AddBinding("<XRController>{LeftHand}/trigger");
            m_gripAction.AddBinding("<XRController>{LeftHand}/gripForce");
        }
        else
        {
            m_pointAction.AddBinding("<XRController>{RightHand}/trigger");
            m_gripAction.AddBinding("<XRController>{RightHand}/gripForce");
        }
        
        m_gripAction.Enable();
        m_pointAction.Enable();

        m_animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Runs input and animate functions
    /// 
    /// note could be changed to fixed update?
    /// </summary>
    private void Update()
    {
        CheckInputs();

        AnimateFinger(m_pointFingers);
        AnimateFinger(m_gripFingers);
    }

    /// <summary>
    /// get the float values from the controls
    /// and pass them to the fingers
    /// </summary>
    private void CheckInputs()
    {
        //check the grip button value

        //if (m_controller.inputDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue)) 
        //if (m_gripAction.ReadValue<float>() > 0)
        //{
        //    SetFingerTargets(m_gripFingers, m_gripAction.ReadValue<float>());
        //}

        ////check the trigger button value
        //if (m_pointAction.ReadValue<float>() > 0)
        //{
        //    SetFingerTargets(m_pointFingers, m_pointAction.ReadValue<float>());
        //}
        SetFingerTargets(m_gripFingers, m_gripAction.ReadValue<float>());
        SetFingerTargets(m_pointFingers, m_pointAction.ReadValue<float>());
        //Debug.Log("Debug " + m_action.ReadValue<float>());
        //SetFingerTargets(m_gripFingers, m_gripAction.action.ReadValue<float>());
        //SetFingerTargets(m_pointFingers, m_action.ReadValue<float>());//m_pointAction.action.ReadValue<float>());
    }

    /// <summary>
    /// Sends the fload values to the list of fingers
    /// </summary>
    /// <param name="fingers">The fingers the values are passed to</param>
    /// <param name="value">The float modifier that defines transition state of the finger</param>
    private void SetFingerTargets(List<Finger> fingers, float value)
    {
        foreach (Finger f in fingers)
        {
            f.TargetMod = value;
        }
    }


    /// <summary>
    /// Smooths the input values and sends the modifiers
    /// to the animator using the target fingers as
    /// keys for the animation masks 
    /// </summary>
    /// <param name="fingers"> A list of fingers to be animated </param>
    private void AnimateFinger(List<Finger> fingers)
    {
        foreach (Finger f in fingers)
        {
            float time = m_transitionSpeed * Time.deltaTime;
            f.CurrentMod = Mathf.MoveTowards(f.CurrentMod, f.TargetMod, time);

            m_animator.SetFloat(f.GetFingerType(), f.CurrentMod);
        }
    }
}

/// <summary>
/// Class that represents an individual finger
/// </summary>
public partial class Finger
{

    public enum FingerType
    {
        None,
        Thumb,
        Index,
        Middle,
        Ring,
        Pinky
    }
    // The type of finger to modify
    private FingerType m_fingerType = FingerType.None;

    //The target animation modifier for the finger
    public float TargetMod { get; set; }
    //the current target modifier for the finger
    public float CurrentMod { get; set; }

    //instantiating the finger
    public Finger(FingerType ft)
    {
        m_fingerType = ft;
    }

    /// <summary>
    /// Gets a string value of the finger type
    /// </summary>
    /// <returns>String name of the finger type</returns>
    public string GetFingerType()
    {
        return m_fingerType.ToString();
    }
}