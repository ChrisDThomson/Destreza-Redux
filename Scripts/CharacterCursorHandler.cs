using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CharacterCursorHandler : CursorHandler
{
    public int playerNumber;
    public PlayerColor playerColor;

    List<Cursor> cursors;
    public int currentCursorIndex;
    public PlayerSettingsInfo playerSetttings;
    public PlayerInputInfo pInputInfo;

    override protected void OnEnable()
    {
        if (cursors == null)
        {
            cursors = new List<Cursor>();

            for (int i = 0; i < playerSetttings.StockCount; i++)
            {
                Cursor c = Instantiate(prefab).GetComponent<Cursor>();
                c.gameObject.SetActive(false);
                c.SetCurrentMenuItemButton(currentMenuItemButton);
                c.ChangeColor(playerColor.playerColor);

                Vector2 offset = Random.insideUnitCircle * 0.35f;
                c.offset = offset;

                c.transform.parent = transform;
                c.onCursorMoved += MovedCursor;

                cursors.Add(c);
            }

            currentCursor = cursors[0];
            if (cursors.Count > 1)
                nextCursor = cursors[1];
            currentCursorIndex = 0;

            cursorSelectionType = (CursorSelection)playerNumber;
        }
        else
        {
            for (int i = 0; i < playerSetttings.StockCount; i++)
            {
                Deselect();
            }

            foreach (Cursor c in cursors)
            {
                c.onCursorMoved -= MovedCursor;
                Destroy(c.gameObject);
            }

            cursors = null;

            //Reinit
            cursors = new List<Cursor>();

            for (int i = 0; i < playerSetttings.StockCount; i++)
            {
                Cursor c = Instantiate(prefab).GetComponent<Cursor>();
                c.gameObject.SetActive(false);
                c.SetCurrentMenuItemButton(currentMenuItemButton);
                c.ChangeColor(playerColor.playerColor);

                Vector2 offset = Random.insideUnitCircle * 0.35f;
                c.offset = offset;

                c.transform.parent = transform;
                c.onCursorMoved += MovedCursor;

                cursors.Add(c);
            }

            currentCursor = cursors[0];
            if (cursors.Count > 1)
                nextCursor = cursors[1];
            currentCursorIndex = 0;

            cursorSelectionType = (CursorSelection)playerNumber;
            
            SetupCurrentCursor();
        }
    }

    protected override void BindPlayerInputs(PlayerInputInfo inputInfo)
    {
        if (didBindInputs)
            return;

        int index = playerNumber - 1;
        if (index > inputInfo.players.Count - 1)
            return;

        if (inputInfo.players.ElementAt(index))
        {
            inputInfo.players[index].onPlayerMoved += Move;
            inputInfo.players[index].onDownButtonPressed += Select;
            inputInfo.players[index].onRightButtonPressed += Deselect;

            currentCursor.ChangeColor(playerColor.playerColor);

            SetupCurrentCursor();

            didBindInputs = true;
        }

        pInputInfo = inputInfo;
    }

    protected override void HandleCursorSelect()
    {
        if (currentCursorIndex + 1 <= cursors.Count - 1)
        {
            base.HandleCursorSelect();
            nextCursor.SetCurrentMenuItemButton(oldCursor.currentMenuItemButton);
            currentCursor = nextCursor;
            currentCursor.gameObject.SetActive(true);
            HighlightCursor();

            currentCursorIndex++;

            if (currentCursorIndex + 1 <= cursors.Count - 1)
                nextCursor = cursors[currentCursorIndex + 1];
            else
                nextCursor = null;

            if (currentCursorIndex - 1 >= 0)
                oldCursor = cursors[currentCursorIndex - 1];
            else
                oldCursor = null;

        }
        else if (currentCursor.isSelected && nextCursor == null)
        {
            currentCursor.ignoreInput = true;
        }
    }

    protected override void HandleCursorDeselect()
    {
        if (currentCursorIndex - 1 >= 0)
        {
            currentCursor = oldCursor;
            currentCursor.ignoreInput = false;

            if (currentCursor.input.magnitude >= input.magnitude)
                currentCursor.input = input;

            currentCursorIndex--;

            if (currentCursorIndex + 1 <= cursors.Count - 1)
                nextCursor = cursors[currentCursorIndex + 1];

            if (currentCursorIndex - 1 >= 0)
                oldCursor = cursors[currentCursorIndex - 1];
            else
                oldCursor = null;

            nextCursor.ResetCursor(currentCursor.currentMenuItemButton);
            nextCursor.gameObject.SetActive(false);
        }

    }

    private void OnDisable()
    {
        // if (pInputInfo == null)
        //     return;

        // int index = playerNumber - 1;
        // if (index > pInputInfo.players.Count - 1)
        //     return;

        // if (pInputInfo.players.ElementAt(index))
        // {
        //     pInputInfo.players[index].onPlayerMoved -= Move;
        //     pInputInfo.players[index].onDownButtonPressed -= Select;
        //     pInputInfo.players[index].onRightButtonPressed -= Deselect;


        //     didBindInputs = false;
        // }

    }


}
