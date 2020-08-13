using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerInput pInput;

    public PlayerInputInfo inputInfo;

    public delegate void OnPlayerMoved(Vector2 mInput);
    public OnPlayerMoved onPlayerMoved;

    public delegate void OnPlayerButtonPress();
    public OnPlayerButtonPress onDownButtonPressed;
    public OnPlayerButtonPress onLeftButtonPressed;
    public OnPlayerButtonPress onRightButtonPressed;
    public OnPlayerButtonPress onUpButtonPressed;

    InputAction upButtonPressed = null;
    InputAction downButtonPressed = null;
    InputAction leftButtonPressed = null;
    InputAction rightButtonPressed = null;

    public OnPlayerButtonPress onDownButtonReleased;
    public OnPlayerButtonPress onLeftButtonReleased;
    public OnPlayerButtonPress onRightButtonReleased;
    public OnPlayerButtonPress onUpButtonReleased;

    void Start()
    {
        FindObjectOfType<Arbiter>().PlayerJoined(pInput.playerIndex, this);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (onPlayerMoved != null)
            onPlayerMoved(context.action.ReadValue<Vector2>());
    }

    #region Up Buttons
    public void OnUpButton(InputAction.CallbackContext context)
    {
        Debug.Log("pressed");
        if (upButtonPressed == null)
        {
            upButtonPressed = context.action;
            upButtonPressed.performed += ctx => UpButtonPressed();
            upButtonPressed.canceled += ctx => UpButtonReleased();
        }
    }

    void UpButtonPressed()
    {
        if (onUpButtonPressed != null)
            onUpButtonPressed();
    }

    void UpButtonReleased()
    {
        if (onUpButtonReleased != null)
            onUpButtonReleased();
    }
    #endregion

    #region  Down Buttons
    public void OnDownButton(InputAction.CallbackContext context)
    {
        if (downButtonPressed == null)
        {
            downButtonPressed = context.action;
            downButtonPressed.performed += ctx => DownButtonPressed();
            downButtonPressed.canceled += ctx => DownButtonReleased();
        }
    }

    void DownButtonPressed()
    {
        if (onDownButtonPressed != null)
            onDownButtonPressed();
    }

    void DownButtonReleased()
    {
        if (onDownButtonReleased != null)
            onDownButtonReleased();
    }

    #endregion

    #region Left Buttons
    public void OnLeftButton(InputAction.CallbackContext context)
    {

        if (leftButtonPressed == null)
        {
            leftButtonPressed = context.action;
            leftButtonPressed.performed += ctx => LeftButtonPressed();
            leftButtonPressed.canceled += ctx => LeftButtonReleased();
        }
    }

    void LeftButtonPressed()
    {
        if (onLeftButtonPressed != null)
            onLeftButtonPressed();
    }

    void LeftButtonReleased()
    {
        if (onLeftButtonReleased != null)
            onLeftButtonReleased();
    }

    #endregion

    #region Right buttons
    public void OnRightButton(InputAction.CallbackContext context)
    {
        if (rightButtonPressed == null)
        {
            rightButtonPressed = context.action;
            rightButtonPressed.performed += ctx => RightButtonPressed();
            rightButtonPressed.canceled += ctx => RightButtonReleased();
        }
    }

    void RightButtonPressed()
    {
        if (onRightButtonPressed != null)
            onRightButtonPressed();
    }

    void RightButtonReleased()
    {
        if (onRightButtonReleased != null)
            onRightButtonReleased();
    }
    #endregion
}
