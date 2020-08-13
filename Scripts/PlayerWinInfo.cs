using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(menuName = "Player/Player Win Info", fileName = "new Player Win Info")]
public class PlayerWinInfo : ScriptableObject
{
    public CharactersWinInfoDictionary characterWinInfo;


    public Stack<WinInfo> P1WinInfo;
    public Stack<WinInfo> P2WinInfo;

    public List<WinInfo> P1CharacterSelection;
    public List<WinInfo> P2CharacterSelection;

    bool isCharacterSelectionLocked;

    List<WinInfo> winningPlayerList;


    //I tried encapsulating but was getting a bunch of null references
    // Because we are only 2 players it should be fine for now
    //But if that changes we need to revisit this
    public void Push(Characters c, int playerIndex)
    {
        if (playerIndex == 0)
        {
            if (P1WinInfo == null)
                P1WinInfo = new Stack<WinInfo>();

            Debug.Log("wininfo" + characterWinInfo[c].name);
            P1WinInfo.Push(characterWinInfo[c]);
        }
        else
        {
            if (P2WinInfo == null)
                P2WinInfo = new Stack<WinInfo>();

            P2WinInfo.Push(characterWinInfo[c]);
        }
    }

    public void Pop(int playerIndex)
    {
        if (isCharacterSelectionLocked)
        {
            Stack<WinInfo> stack = playerIndex == 0 ? ref P1WinInfo : ref P2WinInfo;
            List<WinInfo> listToChange = playerIndex == 0 ? P1CharacterSelection : P2CharacterSelection;

            WinInfo w = stack.Pop();

            WinInfo winInfoInList = listToChange.Find(find => find == w);
            winInfoInList.isKilled = true;
        }
        else
        {
            Stack<WinInfo> stackToChange = playerIndex == 0 ? P1WinInfo : P2WinInfo;

            stackToChange.Pop();
        }
    }

    public void OnSelectionLocked()
    {
        isCharacterSelectionLocked = true;


        //I tried encapsulating but was getting a bunch of null references
        // Because we are only 2 players it should be fine for now
        if (P1CharacterSelection == null)
            P1CharacterSelection = new List<WinInfo>();
        P1CharacterSelection.Clear();

        WinInfo[] arr = new WinInfo[P1WinInfo.Count];
        P1WinInfo.CopyTo(arr, 0);
        for (int i = 0; i < arr.Length; i++)
        {
            Debug.Log(arr[i].ToString());
            P1CharacterSelection.Add(arr[i]);
        }



        if (P2CharacterSelection == null)
            P2CharacterSelection = new List<WinInfo>();
        P2CharacterSelection.Clear();

        WinInfo[] arr1 = new WinInfo[P2WinInfo.Count];
        P2WinInfo.CopyTo(arr1, 0);
        for (int i = 0; i < arr1.Length; i++)
            P2CharacterSelection.Add(arr1[i]);

    }

    public void SetWinningPlayer(int playerIndex)
    {
        Debug.Log("WADSdadsa" + playerIndex);
        if (playerIndex == 1)
            winningPlayerList = P1CharacterSelection;
        else if (playerIndex == 2)
            winningPlayerList = P2CharacterSelection;
        else if (playerIndex == -1)
            //Draw;
            return;

    }

    public List<WinInfo> GetWiningPlayerList()
    {
        if (winningPlayerList == null)
        {
            return P1CharacterSelection;
        }

        return winningPlayerList;
    }

    public void OnLeaveLevel()
    {
        winningPlayerList.Clear();

        P1CharacterSelection.Clear();
        P2CharacterSelection.Clear();

        P1WinInfo?.Clear();
        P2WinInfo?.Clear();
    }
}
