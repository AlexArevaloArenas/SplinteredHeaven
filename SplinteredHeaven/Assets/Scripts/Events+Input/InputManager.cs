using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// This script acts as a single point for all other scripts to get
// the current input from. It uses Unity's new Input System and
// functions should be mapped to their corresponding controls
// using a PlayerInput component with Unity Events.

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{

    private static InputManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Input Manager in the scene.");
        }
        instance = this;
    }

    public static InputManager GetInstance()
    {
        return instance;
    }

    public void OnMove(InputValue _value)
    {
        EventManager.Instance.StartMoveEvent(_value.Get<Vector2>());
    }

    public void OnJump()
    {
        EventManager.Instance.StartJumpEvent();
    }

    public void OnLook(InputValue _value) {
        EventManager.Instance.StartLookEvent(_value.Get<Vector2>());
    }

    public void OnLeftMouse(InputValue _value)
    {
        if (_value.isPressed) //Press
        {
            //Debug.Log("Left Mouse Press");
            EventManager.Instance.StartLeftMouseDownEvent();
        }
        if (_value.isPressed == false) //Release
        {
            //Debug.Log("Left Mouse Release");
            EventManager.Instance.StartLeftMouseUpEvent();
        }
    }

    public void OnLeftMouseHeld(InputValue _value)
    {
        //Debug.Log("Right Mouse Held");
        EventManager.Instance.StartLeftMouseHeldEvent();


    }

    public void OnMouseWheel(InputValue _value)
    {
        //Debug.Log("Mouse Wheel");
        EventManager.Instance.StartMouseWheelEvent(_value.Get<float>());
        //Debug.Log(_value.Get<float>());
    }

    public void OnRightMouse(InputValue _value)
    {
        
        if (_value.isPressed) //Press
        {
            //Debug.Log("Right Mouse Press");
            EventManager.Instance.StartRightMouseDownEvent();
        }
        if (_value.isPressed==false) //Release
        {
            //Debug.Log("Right Mouse Release");
            EventManager.Instance.StartRightMouseUpEvent();
        }

       
    }

    public void OnRightMouseHeld(InputValue _value)
    {
        //Debug.Log("Right Mouse Held");
        EventManager.Instance.StartRightMouseHeldEvent(); 


    }


    /*
    public void OnRightMouseDown(InputValue _value)
    {
        Debug.Log("Right Mouse Down");
        EventManager.Instance.StartLeftMouseEvent();
    }
    public void OnRightMouseUp(InputValue _value)
    {
        Debug.Log("Right Mouse Up");
        EventManager.Instance.StartLeftMouseEvent();
    }
    */

    /*
    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
    }

    public void JumpPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
        }
        else if (context.canceled)
        {
            jumpPressed = false;
        }
    }

    public void InteractButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactPressed = true;
        }
        else if (context.canceled)
        {
            interactPressed = false;
        }
    }

    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
        }
        else if (context.canceled)
        {
            submitPressed = false;
        }
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    // for any of the below 'Get' methods, if we're getting it then we're also using it,
    // which means we should set it to false so that it can't be used again until actually
    // pressed again.

    public bool GetJumpPressed()
    {
        bool result = jumpPressed;
        jumpPressed = false;
        return result;
    }

    public bool GetInteractPressed()
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
    }

    public bool GetSubmitPressed()
    {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }

    public void RegisterSubmitPressed()
    {
        submitPressed = false;
    }
    */
}