using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Courtesy of Ryan Hipple

[System.Serializable]
public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent eventResponse;
    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.DeregisterListener(this);
    }

    public void OnEventRaised()
    {
        eventResponse.Invoke();
    }
}
