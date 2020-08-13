using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(menuName = "Misc/Win Info", fileName = "new Win Info")]
public class WinInfo : ScriptableObject
{
    public Sprite winArt;
    public Sprite winFlower;

    public bool isKilled;

}

[System.Serializable]
public class CharactersWinInfoDictionary : SerializableDictionary<Characters, WinInfo> { }
