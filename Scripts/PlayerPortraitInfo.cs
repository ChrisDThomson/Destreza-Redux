using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(menuName = "Player/Player Portrait Info", fileName = "new Player Portrait Info")]
public class PlayerPortraitInfo : ScriptableObject
{
    public List<List<Sprite>> playerPortraits = null;

    public delegate void OnPortraitChange(int playerindex);

    public OnPortraitChange onPortraitAdded;
    public OnPortraitChange onPortraitRemoved;
    public OnPortraitChange onPortraitHighlighted;

    public void InitData()
    {
        if (playerPortraits == null)
        {
            playerPortraits = new List<List<Sprite>>();

            playerPortraits.Add(new List<Sprite>());
            playerPortraits.Add(new List<Sprite>());
        }

        foreach (List<Sprite> l in playerPortraits)
        {
            l.Clear();
            l.Add(null);
        }
    }

    public void Push(Sprite item, int index)
    {
        playerPortraits[index].Add(item);

        if (onPortraitAdded != null)
            onPortraitAdded(index);
    }

    public void Pop(int index)
    {
        if (playerPortraits[index].Count == 0)
            return;

        playerPortraits[index].RemoveAt(playerPortraits[index].Count - 1);

        if (onPortraitRemoved != null)
            onPortraitRemoved(index);
    }

    public void ChangeLatestPortraitSprite(int index, Sprite sprite)
    {
        if (playerPortraits[index].Count > 0)
        {
            playerPortraits[index][playerPortraits[index].Count - 1] = sprite;

            if (onPortraitHighlighted != null)
                onPortraitHighlighted(index);
        }
    }
}
