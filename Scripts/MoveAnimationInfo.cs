using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(menuName = "Misc/Move Animation Event", fileName = "new Move Animation Event")]
public class MoveAnimationInfo : ScriptableObject
{
    public Vector3 moveAmount;
}
