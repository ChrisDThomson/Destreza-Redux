using UnityEngine;
using UnityEngine.Events;

public class CursorHandler : MonoBehaviour
{
    protected Vector2 input;

    public Cursor nextCursor = null;
    public Cursor currentCursor = null;
    public Cursor oldCursor = null;
    public Cursor prefab;

    public MenuItemButton currentMenuItemButton;

    protected Color cursorColor;

    protected CursorSelection cursorSelectionType = CursorSelection.Unknown;
    protected bool didBindInputs;

    protected virtual void OnEnable()
    {
        currentCursor = Instantiate(prefab, transform.localPosition, Quaternion.identity, transform).GetComponent<Cursor>();
        currentCursor.gameObject.SetActive(false);
        currentCursor.SetCurrentMenuItemButton(currentMenuItemButton);
        currentCursor.onCursorMoved += MovedCursor;
    }


    public void onPlayerJoined(PlayerInputInfo inputInfo)
    {
        BindPlayerInputs(inputInfo);
    }

    public void Move(Vector2 mInput)
    {
        input = mInput;

        if (currentCursor != null)
        {
            currentCursor.input = mInput;
        }
    }

    public virtual void Select()
    {
        if (currentCursor && !currentCursor.isSelected)
        {
            UnityEvent<int> selectedEvent = currentCursor.OnSelect();
            if (selectedEvent == null)
                return;

            Debug.Log(gameObject.name);
            selectedEvent.Invoke((int)cursorSelectionType);
            HandleCursorSelect();
        }
    }

    public virtual void Deselect()
    {
        if (currentCursor && currentCursor.isSelected)
        {
            UnityEvent<int> deselectedEvent = currentCursor.OnDeselect();
            if (deselectedEvent == null)
                return;

            deselectedEvent.Invoke((int)cursorSelectionType);
            if (nextCursor == null)
            {
                currentCursor.ignoreInput = false;
            }
        }
        else if (oldCursor && oldCursor.isSelected)
        {
            UnityEvent<int> deselectedEvent = oldCursor.OnDeselect();
            if (deselectedEvent == null)
                return;

            deselectedEvent.Invoke((int)cursorSelectionType);
            HandleCursorDeselect();
        }
    }

    protected virtual void HandleCursorSelect()
    {
        oldCursor = currentCursor;
        currentCursor.ignoreInput = true;
        currentCursor = null;
    }

    protected virtual void HandleCursorDeselect()
    {
        currentCursor = oldCursor;
        currentCursor.ignoreInput = false;
        oldCursor = null;
    }

    protected virtual void SetupCurrentCursor()
    {
        if (currentCursor != null)
        {
            if (!currentCursor.gameObject.activeSelf)
                currentCursor.gameObject.SetActive(true);

            HighlightCursor();
        }
    }

    protected void MovedCursor(UnityEvent<int> e)
    {
        if (e == null)
            return;

        e.Invoke((int)cursorSelectionType);
    }

    protected virtual void HighlightCursor()
    {
        if (currentCursor == null)
            return;

        UnityEvent<int> e = currentCursor.Highlight();
        if (e != null)
            e.Invoke((int)cursorSelectionType);

    }

    protected virtual void BindPlayerInputs(PlayerInputInfo inputInfo)
    {
        int index = inputInfo.players.Count - 1;

        //add the latest player 
        inputInfo.players[index].onPlayerMoved += Move;
        inputInfo.players[index].onDownButtonPressed += Select;
        inputInfo.players[index].onRightButtonPressed += Deselect;

        SetupCurrentCursor();
    }

    protected enum CursorSelection
    {
        Unknown = 0,
        PlayerOne = 1,
        PlayerTwo = 2
    }
}
