using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

/// <summary>
/// プレイヤー入力を抽象化するインターフェース
/// </summary>
public interface IPlayerInput
{
    Vector2 GetMoveInput();

    Observable<Unit> OnInteract { get; }
}

/// <summary>
/// InputSystemを使用したプレイヤー入力の実装
/// </summary>
public sealed class PlayerInput : IPlayerInput, ITickable
{
    public Observable<Unit> OnInteract => onInteract;

    private Keyboard keyboard;
    private readonly Subject<Unit> onInteract = new Subject<Unit>();

    public void Tick()
    {
        // Input Systemで左クリックを検知
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            onInteract.OnNext(Unit.Default);
        }
    }

    public PlayerInput()
    {
        keyboard = Keyboard.current;
    }

    public Vector2 GetMoveInput()
    {
        if (keyboard == null)
            return Vector2.zero;

        var input = Vector2.zero;

        // WASDキーの入力を取得
        if (keyboard.wKey.isPressed)
            input.y += 1;
        if (keyboard.sKey.isPressed)
            input.y -= 1;
        if (keyboard.dKey.isPressed)
            input.x += 1;
        if (keyboard.aKey.isPressed)
            input.x -= 1;

        return input.normalized;
    }
}
