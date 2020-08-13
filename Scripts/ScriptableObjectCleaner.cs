using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScriptableObjectCleaner : MonoBehaviour
{
    public PlayerCharacterSelectionInfo characterInfo;
    public GameState state;

    //This needs to happen to clean up the character info in case we leave halfway through
    //We could refactor the scriptable object to reset vals on done but this will do for now
    private void OnApplicationQuit()
    {
        characterInfo.OnApplicationQuit();
        state.currentMode = GameStateMode.NotSet;
    }
}
