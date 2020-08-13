using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGameState : MonoBehaviour
{
    public GameState gameState;

    public void ChangeState(GameStateMode mode)
    {
        if(gameState)
        {
            gameState.currentMode = mode;
        }
    }
}
