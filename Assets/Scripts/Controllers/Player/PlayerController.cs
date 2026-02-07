using UnityEngine;
using VContainer.Unity;

/// <summary>
/// プレイヤーのコントローラー
/// WASDキー入力を処理してプレイヤーを制御します
/// </summary>
public sealed class PlayerController : IStartable, ITickable
{
    private readonly PlayerModel model;
    private readonly IPlayerInput input;
    private readonly PlayerView view;
    private readonly CameraView cameraView;
    private readonly Transform lookAtTarget;

    public PlayerController(PlayerModel model, IPlayerInput input, PlayerView view, CameraView cameraView, Transform lookAtTarget)
    {
        this.model = model;
        this.input = input;
        this.view = view;
        this.cameraView = cameraView;
        this.lookAtTarget = lookAtTarget;
    }

    public void Start()
    {
        // ViewにLookAtのターゲットを設定する
        view.LookAtTarget = lookAtTarget;
        view.IsLookAtActive = lookAtTarget != null;
    }

    public void Tick()
    {
        // 入力を取得
        Vector2 moveInput = model.IsInputActive ? input.GetMoveInput() : Vector2.zero;

        // カメラの向きに基づいて移動方向を計算
        // カメラの前方向と右方向を基準にする
        Vector3 cameraForward = cameraView.transform.forward;
        Vector3 cameraRight = cameraView.transform.right;

        // Y成分を0にして、水平方向のみを使用
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        // カメラ基準での移動方向
        Vector3 moveDirection = (cameraRight * moveInput.x + cameraForward * moveInput.y).normalized;

        // モデルを更新（カメラ基準の方向を渡す）
        model.MoveWithDirection(moveDirection, Time.deltaTime);

        // ビューを更新（回転と移動）
        if (moveDirection.magnitude > 0.01f)
        {
            view.UpdateRotate(moveDirection);
        }

        Vector3 motion = model.Velocity * Time.deltaTime;
        view.MoveCharacter(motion);

        // CharacterControllerの実際の位置をモデルに同期
        // これにより壁との衝突など、実際の移動結果がモデルに反映される
        model.SyncPosition(view.transform.position);
    }

    public PlayerModel GetModel() => model;
}
