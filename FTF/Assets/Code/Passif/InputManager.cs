using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    public InputAction ClickAction { get; private set; }
    public InputAction ScrollAction { get; private set; }
    public InputAction PointAction { get; private set; }
    public InputAction ExitAction { get; private set; }

    public bool onClick { get; private set; }
    public Vector2 scroll { get; private set; }
    public Vector2 mousePosition { get; private set; }
    public bool exit { get; private set; }

    public void Initialize()
    {
        InputActionMap map = InputSystem.actions.FindActionMap("UI");
        ClickAction = map.FindAction("Click");
        ScrollAction = map.FindAction("ScrollWheel");
        PointAction = map.FindAction("Point");
        ExitAction = map.FindAction("Cancel");
    }

    public void SetClick(InputAction.CallbackContext context) => onClick = context.ReadValueAsButton();

    public void SetScroll(InputAction.CallbackContext context) => scroll = context.ReadValue<Vector2>();

    public void SetMousePosition(InputAction.CallbackContext context) => mousePosition = context.ReadValue<Vector2>();

    public void SetExit(InputAction.CallbackContext context) => exit = context.ReadValueAsButton();
}
