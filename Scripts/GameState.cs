using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(menuName = "GameState", fileName = "new Game State")]
public class GameState : ScriptableObject
{
    public GameStateMode currentMode;

}

[System.Serializable]
public enum GameStateMode
{
    Arcade,
    Versus,
    WorldTour,
    Training,
    Online,


    NotSet
}
