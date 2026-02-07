using UnityEngine;

/// <summary>
/// プレイヤーのモデル
/// プレイヤーの位置、速度などのロジックを管理します
/// </summary>
public sealed class PlayerModel
{
    private Vector3 position;
    private Vector3 velocity;
    private float moveSpeed = 5f;
    private float acceleration = 20f;
    private float deceleration = 15f;

    public Vector3 Position => position;
    public Vector3 Velocity => velocity;
    public bool IsInputActive { get; set; } = true;

    public PlayerModel(Vector3 startPosition)
    {
        this.position = startPosition;
        this.velocity = Vector3.zero;
    }

    /// <summary>
    /// 移動入力からプレイヤーを移動（レガシー）
    /// </summary>
    public void Move(Vector2 moveInput, float deltaTime)
    {
        // 入力を3D方向に変換（X=右、Z=前）
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        // 目標速度を計算
        Vector3 targetVelocity = moveDirection * moveSpeed;

        // 現在の速度から目標速度へスムーズに遷移
        if (moveInput.magnitude > 0.01f)
        {
            velocity = Vector3.Lerp(velocity, targetVelocity, acceleration * deltaTime);
        }
        else
        {
            velocity = Vector3.Lerp(velocity, Vector3.zero, deceleration * deltaTime);
        }

        // 位置を更新
        position += velocity * deltaTime;
    }

    /// <summary>
    /// 3D方向ベクトルに基づいて移動
    /// カメラ基準の方向など、既に計算された方向を受け取る
    /// </summary>
    public void MoveWithDirection(Vector3 direction, float deltaTime)
    {
        // 目標速度を計算
        Vector3 targetVelocity = direction * moveSpeed;

        // 現在の速度から目標速度へスムーズに遷移
        if (direction.magnitude > 0.01f)
        {
            velocity = Vector3.Lerp(velocity, targetVelocity, acceleration * deltaTime);
        }
        else
        {
            velocity = Vector3.Lerp(velocity, Vector3.zero, deceleration * deltaTime);
        }

        // 位置を更新
        position += velocity * deltaTime;
    }

    /// <summary>
    /// 移動速度を設定
    /// </summary>
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    /// <summary>
    /// 実際の位置を同期（CharacterControllerの衝突判定後の位置）
    /// </summary>
    public void SyncPosition(Vector3 actualPosition)
    {
        position = actualPosition;
    }
}
