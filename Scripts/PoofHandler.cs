using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "PoofHandler", menuName = "Poofs/PoofHandler", order = 1)]
public class PoofHandler : ScriptableObject
{
	[Range(-5,5)]
    public float scale = 1;
    public Vector3 position = Vector3.zero;

    public Poofs poofID = Poofs.Poof_NotSet;
}

public enum Poofs
{

    Poof_NotSet,
    Poof_Small,
    Poof_Run,
    Poof_Jump,
    Poof_Dash,
    Poof_Puff,

    Poof_Count
}
