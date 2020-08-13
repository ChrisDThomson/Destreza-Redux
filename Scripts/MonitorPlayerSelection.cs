using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorPlayerSelection : MonoBehaviour
{
    public PlayerCharacterSelectionInfo playerCharacterInfo;
    public PlayerSettingsInfo playerSettingsInfo;

    public GameEvent playersSelected;
    public GameEvent playersCancelSelected;

    bool havePlayersSelected;
    public int numberOfPlayersToCheck = 1;

    private void Start()
    {
        playerCharacterInfo.onCharacterPush += CheckPlayerSelection;
        playerCharacterInfo.onCharacterPop += PlayersCancelSelection;
    }

    public void CheckPlayerSelection(int playerIndex)
    {
        int stockCount = playerSettingsInfo.StockCount;

        if (playerCharacterInfo.playerCharacters.Count != numberOfPlayersToCheck)
            return;

        int playersAtMaxSelection = 0;
        foreach (var p in playerCharacterInfo.playerCharacters)
        {
            if (p.Count == stockCount)
                playersAtMaxSelection++;
        }

        if (playersAtMaxSelection == numberOfPlayersToCheck)
        {
            playersSelected.Raise();
            havePlayersSelected = true;
        }
    }

    public void PlayersCancelSelection(int playerIndex)
    {
        if (!havePlayersSelected)
            return;

        havePlayersSelected = false;
        playersCancelSelected.Raise();
    }
}
