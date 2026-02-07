using UnityEngine;
using VContainer.Unity;

/// <summary>
/// オービットカメラのコントローラー
/// DIで注入されたマウス入力とモデルを使用してカメラを制御します
/// MonoBehaviourには依存しません
/// </summary>
public sealed class OrbitCameraController : IStartable, ILateTickable
{
    private readonly OrbitCameraModel model;
    private readonly IMouseInput mouseInput;
    private readonly CameraView view;
    private readonly PlayerView playerView;
    private readonly float horizontalSensitivity;
    private readonly float verticalSensitivity;
    private readonly float zoomSensitivity;

    public OrbitCameraController(
        OrbitCameraModel model,
        IMouseInput mouseInput,
        CameraView view,
        PlayerView playerView,
        float horizontalSensitivity = 2f,
        float verticalSensitivity = 2f,
        float zoomSensitivity = 2f)
    {
        this.model = model;
        this.mouseInput = mouseInput;
        this.view = view;
        this.playerView = playerView;
        this.horizontalSensitivity = horizontalSensitivity;
        this.verticalSensitivity = verticalSensitivity;
        this.zoomSensitivity = zoomSensitivity;
    }

    public void Start()
    {
        // 初期化処理（必要に応じて）
    }

    public void LateTick()
    {   
        // プレイヤーのGameObjectの実際の位置にカメラのターゲットを更新
        // CharacterControllerによる衝突判定の結果が反映された位置を使用
        model.SetTargetPosition(playerView.transform.position);

        // マウスドラッグでカメラを回転
        if (mouseInput.IsOrbitButtonPressed())
        {
            var mouseDelta = mouseInput.GetMouseDelta();
            model.Rotate(mouseDelta.x * horizontalSensitivity, -mouseDelta.y * verticalSensitivity);
        }

        // マウスホイールでズーム
        float scrollDelta = mouseInput.GetScrollDelta();
        if (scrollDelta != 0f)
        {
            model.Zoom(scrollDelta * zoomSensitivity);
        }

        // View に反映
        view.UpdateCamera(model.Position, model.Rotation);
    }

    public OrbitCameraModel GetModel() => model;
}
