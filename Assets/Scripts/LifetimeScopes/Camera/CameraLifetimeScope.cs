using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class CameraLifetimeScope : LifetimeScope
{
    [SerializeField] private float initialDistance = 10f;
    [SerializeField] private float horizontalSensitivity = 2f;
    [SerializeField] private float verticalSensitivity = 2f;
    [SerializeField] private float zoomSensitivity = 2f;

    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float minVerticalAngle = -85f;
    [SerializeField] private float maxVerticalAngle = 85f;

    [SerializeField] private PlayerView playerView;
    [SerializeField] private CameraView cameraView;

    protected override void Configure(IContainerBuilder builder)
    {
        // IMouseInput の登録
        builder.Register<IMouseInput, MouseInput>(Lifetime.Singleton);

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
                container.Resolve<IMouseInput>(),
                cameraView,
                playerView,
                horizontalSensitivity,
                verticalSensitivity,
                zoomSensitivity),
            Lifetime.Singleton);

        // CameraView の登録
        builder.RegisterComponent(cameraView);
    }
}
