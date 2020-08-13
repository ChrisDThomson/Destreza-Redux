using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Courtesy of Ryan Hipple

[CreateAssetMenu(menuName = "Game Event", fileName = "new Game Event")]
public class GameEvent : ScriptableObject
{
    List<GameEventListener> listeners;

    public void Raise()
    {
        foreach (GameEventListener l in listeners)
        {
            l.OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener gameEventListener)
    {
        if (listeners == null)
            listeners = new List<GameEventListener>();

        listeners.Add(gameEventListener);
    }

    public void DeregisterListener(GameEventListener gameEventListener)
    {
        listeners.Remove(gameEventListener);
    }
}
