using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// マウス入力を抽象化するインターフェース
/// </summary>
public interface IMouseInput
{
    Vector2 GetMouseDelta();
    float GetScrollDelta();
    bool IsOrbitButtonPressed();
}

/// <summary>
/// InputSystemを使用したマウス入力の実装
/// </summary>
public sealed class MouseInput : IMouseInput
{
    private Mouse mouse;

    public MouseInput()
    {
        mouse = Mouse.current;
    }

    public Vector2 GetMouseDelta()
    {
        if (mouse == null)
            return Vector2.zero;

        return mouse.delta.ReadValue();
    }

    public float GetScrollDelta()
    {
        if (mouse == null)
            return 0f;

        return mouse.scroll.ReadValue().y;
    }

    public bool IsOrbitButtonPressed()
    {
        if (mouse == null) {
            return false;
        }
        return mouse.rightButton.isPressed;
    }
}
