using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(menuName = "Player/Player Color", fileName = "new Player Color Info")]
public class PlayerColor : ScriptableObject
{
    public Color playerColor = Color.white;
    public Color windowBGColor = Color.white;

}
