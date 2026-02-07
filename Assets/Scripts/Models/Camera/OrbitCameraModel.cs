using UnityEngine;

/// <summary>
/// オービットカメラのモデル
/// ターゲットの周りを移動するカメラのロジックを管理します
/// </summary>
public sealed class OrbitCameraModel
{
    private Vector3 targetPosition;
    private float distance;
    private float horizontalAngle; // X軸周りの回転（ヨー）
    private float verticalAngle;   // Y軸周りの回転（ピッチ）
    private Quaternion rotation;
    private Vector3 position;

    // 制約値
    private float minDistance = 2f;
    private float maxDistance = 20f;
    private float minVerticalAngle = -85f;
    private float maxVerticalAngle = 85f;
    private float lookAtOffsetHeight = 1.5f; // ターゲットより上の注目点

    public Vector3 Position => position;
    public Quaternion Rotation => rotation;
    public Vector3 TargetPosition => targetPosition;
    public float Distance => distance;
    public float HorizontalAngle => horizontalAngle;
    public float VerticalAngle => verticalAngle;

    public OrbitCameraModel(Vector3 targetPosition, float distance = 10f)
    {
        this.targetPosition = targetPosition;
        this.distance = Mathf.Clamp(distance, minDistance, maxDistance);
        this.horizontalAngle = 0f;
        this.verticalAngle = 20f; // 少し上から見下ろす角度

        UpdateCameraTransform();
    }

    /// <summary>
    /// マウスの移動量からカメラの角度を更新
    /// </summary>
    public void Rotate(float deltaHorizontal, float deltaVertical)
    {
        horizontalAngle += deltaHorizontal;
        verticalAngle += deltaVertical;

        // 垂直角度を制約
        verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle);

        // 水平角度は360度で一周
        horizontalAngle = horizontalAngle % 360f;

        UpdateCameraTransform();
    }

    /// <summary>
    /// マウスホイールからカメラの距離を更新
    /// </summary>
    public void Zoom(float delta)
    {
        distance -= delta;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        UpdateCameraTransform();
    }

    /// <summary>
    /// ターゲット位置を更新
    /// </summary>
    public void SetTargetPosition(Vector3 position)
    {
        this.targetPosition = position;
        UpdateCameraTransform();
    }

    /// <summary>
    /// カメラの位置と回転を計算
    /// </summary>
    private void UpdateCameraTransform()
    {
        // 注目点を計算（ターゲットより少し上）
        Vector3 lookAtPoint = targetPosition + Vector3.up * lookAtOffsetHeight;

        // 角度をラジアンに変換
        float horRad = horizontalAngle * Mathf.Deg2Rad;
        float verRad = verticalAngle * Mathf.Deg2Rad;

        // 球座標からカメラ位置を計算（注目点を中心とする）
        float x = distance * Mathf.Cos(verRad) * Mathf.Sin(horRad);
        float y = distance * Mathf.Sin(verRad);
        float z = distance * Mathf.Cos(verRad) * Mathf.Cos(horRad);

        position = lookAtPoint + new Vector3(x, y, z);

        // カメラが注目点を向くように回転を設定
        Vector3 direction = lookAtPoint - position;
        rotation = Quaternion.LookRotation(direction);
    }

    public void SetConstraints(float minDist, float maxDist, float minVert, float maxVert)
    {
        minDistance = minDist;
        maxDistance = maxDist;
        minVerticalAngle = minVert;
        maxVerticalAngle = maxVert;

        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle);

        UpdateCameraTransform();
    }
}
