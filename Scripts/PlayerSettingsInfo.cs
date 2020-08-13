using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(menuName = "Player/Settings Info", fileName = "new PlayerSettingsInfo")]
public class PlayerSettingsInfo : ScriptableObject
{
    [Range(1, 7)]
    public int StockCount = 4;
}
