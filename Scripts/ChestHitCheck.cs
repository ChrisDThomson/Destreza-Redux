using UnityEngine;
using System.Collections;

public class ChestHitCheck : MonoBehaviour
{
    public delegate void OnFatalHit();
    public OnFatalHit onFatalHit;

    void OnTriggerEnter2D(Collider2D col)
    {
        //we've hit ourselves
        if (col.gameObject.transform.parent == this.gameObject.transform.parent)
            return;

        //Different effects depending on what hit us
        if (col.CompareTag("RedUpInD"))
        {
            WasFatallyHit();
            //Instantiate(bloodHit, col.transform.position, Quaternion.identity);
        }
        else if (col.CompareTag("RedUpInF"))
        {
            WasFatallyHit();
            //Instantiate(bloodHit, col.transform.position, Quaternion.identity);
        }
        else if (col.CompareTag("RedDownOutD"))
        {
            WasFatallyHit();
            //Instantiate(bloodHit, col.transform.position, Quaternion.identity);
        }
        else if (col.CompareTag("RedDownOutF"))
        {
            WasFatallyHit();
            //Instantiate(bloodHit, col.transform.position, Quaternion.identity);
        }
        else if (col.CompareTag("Thrust"))
        {
            WasFatallyHit();
            //Instantiate(bloodThrust, character.isOnLeftSide ? character.transform.position + new Vector3(-0.0f, 3f) : character.transform.position + new Vector3(0.0f, 3f), character.isOnLeftSide ? Quaternion.identity : Quaternion.Euler(new Vector3(0, 180, 0)));
        }
    }

    void WasFatallyHit()
    {
        if (onFatalHit != null)
            onFatalHit();
    }
}

