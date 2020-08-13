using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(menuName = "Player/Player Character Info", fileName = "new Player Character Info")]
public class PlayerCharacterSelectionInfo : ScriptableObject
{

    public PlayerWinInfo playerWinInfo;
    public List<Stack<Character>> playerCharacters;


    //all this fluff is to be able to view in inspector
    public List<List<Character>> debugView;

    public List<Character> Player1List;

    public List<Character> Player2List;


    public delegate void OnCharacterRosterChange(int playerindex);

    public OnCharacterRosterChange onCharacterPush;
    public OnCharacterRosterChange onCharacterPop;

    public delegate void CharacterPopped(Character character, int playerIndex);
    public CharacterPopped poppedCharacter;


    public void Push(Character item, int index)
    {
        playerCharacters[index].Push(item);

        //This is so we can view it at runtime in the inspector
#if UNITY_EDITOR
        debugView[index].Clear();
        foreach (Character s in playerCharacters[index])
        {
            debugView[index].Add(s);
        }

        Player1List = debugView[0];
        Player2List = debugView[1];
#endif

        if (onCharacterPush != null)
            onCharacterPush(index);
    }

    public void Pop(int index)
    {
        if (playerCharacters[index].Count == 0)
        {
            if (poppedCharacter != null)
                poppedCharacter(null, index);
            return;
        }

        Character c = playerCharacters[index].Pop();
        if (poppedCharacter != null)
            poppedCharacter(c, index);

        //This is so we can view it at runtime in the inspector
#if UNITY_EDITOR
        debugView[index].Clear();
        foreach (Character s in playerCharacters[index])
        {
            debugView[index].Add(s);
        }

        Player1List = debugView[0];
        Player2List = debugView[1];
#endif

        if (onCharacterPop != null)
            onCharacterPop(index);

        //playerWinInfo.Pop(index);
    }

    public void OnApplicationQuit()
    {
        if (playerCharacters == null)
        {
            playerCharacters = new List<Stack<Character>>();
            playerCharacters.Add(new Stack<Character>());
            playerCharacters.Add(new Stack<Character>());
        }

        if (debugView == null)
        {
            debugView = new List<List<Character>>();
            debugView.Add(new List<Character>());
            debugView.Add(new List<Character>());
        }

        if (Player1List == null)
            Player1List = new List<Character>();

        if (Player2List == null)
            Player2List = new List<Character>();

        foreach (Stack<Character> s in playerCharacters)
        {
            s.Clear();
        }

        foreach (List<Character> l in debugView)
        {
            l.Clear();
        }

        Player1List.Clear();
        Player2List.Clear();
    }



    //This is for setting the characters to spawn for test scenes
    public void OnDebug()
    {

        if (playerCharacters == null)
        {
            playerCharacters = new List<Stack<Character>>();
            playerCharacters.Add(new Stack<Character>());
            playerCharacters.Add(new Stack<Character>());
        }

        if (debugView == null)
        {
            debugView = new List<List<Character>>();
            debugView.Add(new List<Character>());
            debugView.Add(new List<Character>());
        }

        foreach (Stack<Character> s in playerCharacters)
        {
            s.Clear();
        }

        foreach (Character c in Player1List)
        {
            playerCharacters[0].Push(c);
        }

        foreach (Character c in Player2List)
        {
            playerCharacters[1].Push(c);
        }
    }

    public void OnLeaveLevel()
    {
        foreach (var s in playerCharacters)
        {
            s.Clear();
        }
    }
}
