using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinHandler : MonoBehaviour
{
    public MannequinInfo mannequinInfo;

    public int playerNumber;

    public CharactersMannequinDictionary mannequins;

    public Mannequin activeMannequin;

    private void Start()
    {
        mannequinInfo.onCursorHighlightChange += UpdateMannequinDisplay;

        mannequins.CopyFrom(mannequinInfo.mannequins);
        foreach (var m in mannequinInfo.mannequins)
        {
            Mannequin man = Instantiate(m.Value.gameObject).GetComponent<Mannequin>();
            man.transform.position = transform.position;
            man.transform.parent = transform;
            man.gameObject.SetActive(false);

            mannequins[m.Key] = man;
        }

        if (playerNumber % 2 == 0)
        {
            Vector3 scale = transform.lossyScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    public void UpdateMannequinDisplay(int playerIndex, Characters c)
    {
        if (playerNumber != playerIndex)
            return;

        foreach (var m in mannequins.Keys)
        {
            if (m == c)
                mannequins[m].gameObject.SetActive(true);
            else
                mannequins[m].gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
       // mannequinInfo.onCursorHighlightChange -= UpdateMannequinDisplay;
    }
}
