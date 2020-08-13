using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portrait : MonoBehaviour
{
    public SpriteRenderer characterPortrait;
    public SpriteRenderer windowBG;
    public SpriteRenderer numberBG;

    public SpriteRenderer swordCursor;

    Vector3 horiSwordPos = new Vector3(-2, 0, 0);
    Vector3 horiSwordEuler = new Vector3(0, 0, -90);

    Vector3 rotatedSwordPos = new Vector3(-1.7f, -1, 0);
    Vector3 rotatedSwordEuler = new Vector3(0, 0, -60);

    public void SetSwordOrientation(bool isHorizontal)
    {
        if (isHorizontal)
        {
            swordCursor.gameObject.transform.localPosition = horiSwordPos;
            swordCursor.gameObject.transform.localRotation = Quaternion.Euler(horiSwordEuler);
        }
        else
        {
            swordCursor.gameObject.transform.localPosition = rotatedSwordPos;
            swordCursor.gameObject.transform.localRotation = Quaternion.Euler(rotatedSwordEuler);
        }
    }

    public void SetSwordColor(Color color)
    {
        swordCursor.color = color;
    }

    public void SetWindowBGColor(Color color)
    {
        windowBG.color = color;
    }

    public void SetSwordVisiblity(bool isVisible)
    {
        swordCursor.enabled = isVisible;
    }
}
