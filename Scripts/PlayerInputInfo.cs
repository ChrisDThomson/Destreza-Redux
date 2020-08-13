using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(menuName = "Player/Player Input Info", fileName = "new Player Input Info")]
public class PlayerInputInfo : ScriptableObject
{
    public List<Player> players;

    public void Reset()
    {
        players.Clear();
    }
}
