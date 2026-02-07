using UnityEngine;
using VContainer;
using VContainer.Unity;

/// <summary>
/// プレイヤーとカメラシステムのDI設定
/// </summary>
public sealed class PlayerLifetimeScope : LifetimeScope
{
    [SerializeField] private PlayerView playerView;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform lookAtTarget;

    [SerializeField] private float initialDistance = 10f;
    [SerializeField] private float horizontalSensitivity = 2f;
    [SerializeField] private float verticalSensitivity = 2f;
    [SerializeField] private float zoomSensitivity = 2f;

    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float minVerticalAngle = -85f;
    [SerializeField] private float maxVerticalAngle = 85f;

    [SerializeField] private CameraView cameraView;

    protected override void Configure(IContainerBuilder builder)
    {
        // ===== プレイヤーシステム (シーン固有) =====
        
        // PlayerModel の登録
        builder.Register<PlayerModel>(
            _ => {
                var model = new PlayerModel(playerView != null ? playerView.transform.position : Vector3.zero);
                model.SetMoveSpeed(moveSpeed);
                return model;
            },
            Lifetime.Singleton);

        // PlayerController の登録
        builder.RegisterEntryPoint<PlayerController>(
            container => new PlayerController(
                container.Resolve<PlayerModel>(),
                container.Resolve<IPlayerInput>(),
                playerView,
                cameraView,
                lookAtTarget),
            Lifetime.Singleton).AsSelf();

        // PlayerView の登録
        builder.RegisterComponent(playerView);

        // ===== カメラシステム (シーン固有) =====
        // OrbitCameraModel の登録
        builder.Register<OrbitCameraModel>(container =>
        {
            var model = new OrbitCameraModel(
                playerView != null ? playerView.transform.position : Vector3.zero,
                initialDistance);

            // 制約値の設定
            typeof(OrbitCameraModel)
                .GetField("minDistance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(model, minDistance);
            typeof(OrbitCameraModel)
                .GetField("maxDistance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(model, maxDistance);
            typeof(OrbitCameraModel)
                .GetField("minVerticalAngle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(model, minVerticalAngle);
            typeof(OrbitCameraModel)
                .GetField("maxVerticalAngle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(model, maxVerticalAngle);

            return model;
        }, Lifetime.Singleton);

        // OrbitCameraController の登録
        builder.RegisterEntryPoint<OrbitCameraController>(
            container => new OrbitCameraController(
                container.Resolve<OrbitCameraModel>(),
                // IMouseInputはProjectLifetimeScopeから解決
                container.Resolve<IMouseInput>(),
                cameraView,
                playerView,
                horizontalSensitivity,
                verticalSensitivity,
                zoomSensitivity),
            Lifetime.Singleton);

        // CameraView の登録
        builder.RegisterComponent(cameraView);

        // ===== インタラクションシステム (シーン固有) =====
        // PlayerInteractionControllerはPlayerView/PlayerModelに依存するため、シーンスコープで登録
        builder.RegisterEntryPoint<PlayerInteractionController>(Lifetime.Singleton).AsSelf();

        // PlayerInput登録
        builder.Register<PlayerInput>(Lifetime.Singleton).AsImplementedInterfaces();
    }
}
