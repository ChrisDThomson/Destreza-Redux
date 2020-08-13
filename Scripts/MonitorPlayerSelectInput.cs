using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonitorPlayerSelectInput : MonoBehaviour
{
    //not great but will do for now
    public int playerNumber = 1;

    public UnityEvent eventOnSelect;
    public void OnPlayerJoined(PlayerInputInfo inputInfo)
    {
        if (playerNumber <= 2)
        {
            inputInfo.players[playerNumber - 1].onDownButtonPressed += OnPlayerSelect;

            playerNumber++;
        }
    }

    public void OnPlayerSelect()
    {
        if (SceneCoordinator.CurrentSceneName != gameObject.scene.name)
            return;

        if (eventOnSelect != null)
            eventOnSelect.Invoke();
    }
}
