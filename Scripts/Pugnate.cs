using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pugnate : MonoBehaviour
{
    public OnAnimationFinished onAnimationFinished;
    public Animator animator;

    public GameEvent PugnateDissapeared;
    // Start is called before the first frame update
    void Start()
    {
        onAnimationFinished.onAnimationFinished += onDissapear;
    }

    public void OnNewRound()
    {
        animator.SetTrigger("Disappear");
    }

    void onDissapear()
    {
        PugnateDissapeared.Raise();
    }
}
