using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField]
    private UnityEvent event0;

    public void Event0()
    {
        event0.Invoke();
    }
}
