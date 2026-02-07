using UnityEngine;

/// <summary>
/// オービットカメラのビュー
/// モデルの位置と回転をUnityのカメラに反映させます
/// </summary>
public sealed class CameraView : MonoBehaviour
{
    public void UpdateCamera(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }
}
