using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuItemButton : MonoBehaviour
{
    public MenuItemButtonDirections menuItemButtonDirections;
    public bool canSelect = true;

    public bool canHighlight = true;

    [SerializeField]
    UnityEvent<int> onSelect;

    [SerializeField]
    UnityEvent<int> onDeselect;
    [SerializeField]
    UnityEvent<int> onHighlight = null;
    public MenuItemButton GetButtonFromInput(Vector2 input)
    {
        if (input.y > 0.5f)
            return menuItemButtonDirections.above;

        if (input.y < -0.5f)
            return menuItemButtonDirections.under;

        if (input.x > 0.5f)
            return menuItemButtonDirections.right;

        if (input.x < -0.5f)
            return menuItemButtonDirections.left;

        return null;
    }

    public UnityEvent<int> Select()
    {
        if (canSelect)
        {
            return onSelect;
        }

        return null;
    }

    public UnityEvent<int> Deselect()
    {
        if (canSelect)
        {
            return onDeselect;
        }

        return null;
    }

    public UnityEvent<int> Highlight()
    {
        return onHighlight;
    }


    [System.Serializable]
    public struct MenuItemButtonDirections
    {
        public MenuItemButton above;
        public MenuItemButton under;
        public MenuItemButton left;
        public MenuItemButton right;
    }
}
