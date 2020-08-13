using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacterRoster : MonoBehaviour
{
    public PlayerWinInfo playerWinInfo;
    public PlayerCharacterSelectionInfo playerCharacterInfo;
    public PlayerPortraitInfo playerPortraitInfo;
    public MannequinInfo mannequinInfo;

    public Character character;
    public Characters characterType;

    public Sprite characterPortraitArt;

    public void AddCharacterToRoster(int playerNumber)
    {
        if (playerCharacterInfo.playerCharacters == null)
        {
            playerCharacterInfo.playerCharacters = new List<Stack<Character>>();
            playerCharacterInfo.playerCharacters.Add(new Stack<Character>());
            playerCharacterInfo.playerCharacters.Add(new Stack<Character>());
        }

        if (playerNumber - 1 >= 0 && playerNumber - 1 <= playerCharacterInfo.playerCharacters.Count - 1)
            playerCharacterInfo.Push(character, playerNumber - 1);
        else
            Debug.Log("Not a valid index! : " + playerNumber);


        if (playerNumber - 1 >= 0 && playerNumber - 1 <= playerPortraitInfo.playerPortraits.Count - 1)
            playerPortraitInfo.Push(characterPortraitArt, playerNumber - 1);
        else
            Debug.Log("Not a valid index! : " + playerNumber);

        playerWinInfo.Push(characterType, playerNumber - 1);

    }

    public void RemoveFromCharacterToRoster(int playerNumber)
    {
        if (playerNumber - 1 >= 0 && playerNumber - 1 <= playerCharacterInfo.playerCharacters.Count - 1)
            playerCharacterInfo.Pop(playerNumber - 1);
        else
            Debug.Log("Not a valid index! : " + playerNumber);


        if (playerNumber - 1 >= 0 && playerNumber - 1 <= playerPortraitInfo.playerPortraits.Count - 1)
            playerPortraitInfo.Pop(playerNumber - 1);
        else
            Debug.Log("Not a valid index! : " + playerNumber);
    }

    public void HighlightCharacter(int playerNumber)
    {
        playerPortraitInfo.ChangeLatestPortraitSprite(playerNumber - 1, characterPortraitArt);
        mannequinInfo.Highlight(playerNumber, characterType);
    }
}
