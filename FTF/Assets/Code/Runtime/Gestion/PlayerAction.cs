using UnityEngine;
using static GameManager;

public class PlayerAction
{
    IInteractable interactable;
    bool wasClicking;
    Vector2 lastMousePosition;

    public void Initialize()
    {

    }

    public void ActionUpdate()
    {
        InputManager input = GM.inputManager;

        if (input.onClick && !wasClicking)
        {
            Collider2D hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(input.mousePosition));
            if (hit && hit.TryGetComponent(out interactable))
                interactable.OnTouch();
        }
        else if (input.onClick && interactable != null)
        {
            if (input.mousePosition != lastMousePosition)
                interactable.OnDrag();
            else
                interactable.OnHold();
        }
        else if (!input.onClick && interactable != null)
        {
            interactable.OnRelease();
            interactable = null;
        }

        wasClicking = input.onClick;
        lastMousePosition = input.mousePosition;
    }
}
