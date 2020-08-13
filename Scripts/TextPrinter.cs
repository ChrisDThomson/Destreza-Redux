using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPrinter : MonoBehaviour
{
    public string Text;

    public void Print()
    {
        Debug.Log(Text);
    }
}
