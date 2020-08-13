using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(menuName = "Misc/Mannequin Info", fileName = "new Mannequin Info")]
public class MannequinInfo : ScriptableObject
{
    public CharactersMannequinDictionary mannequins;

    public delegate void OnHighlightChange(int playerNumber, Characters characters);
    public OnHighlightChange onCursorHighlightChange;

    public MannequinInfo()
    {
        mannequins = new CharactersMannequinDictionary();
    }

    public void Highlight(int playerNumber, Characters c)
    {
        if (onCursorHighlightChange != null)
            onCursorHighlightChange(playerNumber, c);
    }
}


[System.Serializable]
public class CharactersMannequinDictionary : SerializableDictionary<Characters, Mannequin> { }

