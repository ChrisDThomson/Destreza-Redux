using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cursor : MonoBehaviour
{
    public MenuItemButton currentMenuItemButton;

    public Vector2 input;
    public bool ignoreInput;
    public bool isSelected;

    float timer = 0;
    float timerTime = 0.2f;
    public Animator animator;

    public Vector3 selectionOffset;
    public Vector3 offset;
    public Vector3 selectPos;

    UnityEvent<int> highlightEvent;
    public SpriteRenderer spriteRenderer;

    public delegate void OnCursorMoved(UnityEvent<int> e);
    public OnCursorMoved onCursorMoved;

    // Update is called once per frame
    void Update()
    {
        if (currentMenuItemButton == null)
            return;

        animator.SetFloat("moveMagnitude", input.magnitude);

        if (timer > 0)
            timer -= Time.deltaTime;
        else if (timer < 0)
            timer = 0;

        if (timer == 0 && !ignoreInput)
        {
            MenuItemButton newButton = currentMenuItemButton.GetButtonFromInput(input);

            if (newButton)
            {
                SetCurrentMenuItemButton(newButton);
                timer = timerTime;
            }
        }
    }

    public void SetCurrentMenuItemButton(MenuItemButton button)
    {
        currentMenuItemButton = button;
        transform.position = currentMenuItemButton.transform.position + selectionOffset;

        if (onCursorMoved != null)
            onCursorMoved(currentMenuItemButton.Highlight());


    }

    public UnityEvent<int> Highlight()
    {
        return currentMenuItemButton.Highlight();
    }

    public void ResetCursor(MenuItemButton button)
    {
        SetCurrentMenuItemButton(button);
        input = Vector2.zero;
        animator.SetTrigger("ReturnToEntry");
    }

    public virtual UnityEvent<int> OnSelect()
    {

        UnityEvent<int> eventOnSelect = currentMenuItemButton.Select();
        if (eventOnSelect != null)
        {
            isSelected = true;
            selectPos = transform.position;

            animator.SetTrigger("Selected");
            //float time = 0;
            Vector3 target = transform.position + offset + selectionOffset;
            transform.position = target;
            // Vector3 target = transform.position + selectPos + selectionOffset;
            // while (time <= 1)
            // {
            //     transform.position = Vector3.Lerp(transform.position, target, time);
            //     time += Time.deltaTime;
            // }

            return eventOnSelect;
        }
        else
        {
            return null;
        }
    }

    public virtual UnityEvent<int> OnDeselect()
    {
        isSelected = false;
        UnityEvent<int> eventOnSelect = currentMenuItemButton.Deselect();
        timer = 0;
        if (eventOnSelect != null)
        {

            animator.SetTrigger("Deselected");
            float time = 0;
            while (time <= 1)
            {
                transform.position = Vector3.Lerp(transform.position + selectionOffset, selectPos, time);
                time += Time.deltaTime;
            }
            return eventOnSelect;
        }
        else
        {
            return null;
        }
    }


    public void ChangeColor(Color color)
    {
        spriteRenderer.color = color;
    }
}
