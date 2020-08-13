using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnAnimationFinished : MonoBehaviour
{
    public Animator animator;
    public UnityEvent eventOnFinished;
    public delegate void WhenAnimationFinished();
    public WhenAnimationFinished onAnimationFinished;

    public void AnimationFinished()
    {
        if (onAnimationFinished != null)
            onAnimationFinished();
    }
}

