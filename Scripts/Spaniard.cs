using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaniard : Character
{
    protected override void HitWallBack()
    {
        animator.SetTrigger("wallbump");
    }
}

