using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ProjectLifetimeScope : LifetimeScope
{
    [SerializeField] private FadeView fadeView;
    [SerializeField] private TextView textView;
    [SerializeField] private SoundView soundView;

    protected override void Configure(IContainerBuilder builder)
    {
        // ===== 共通システム (プロジェクト全体) =====

        builder.RegisterComponent(fadeView);
        builder.RegisterComponent(textView);
        builder.RegisterComponent(soundView);

        // FadeModel の登録
        builder.Register<FadeModel>(Lifetime.Singleton);

        // FadeController の登録
        builder.RegisterEntryPoint<FadeController>(Lifetime.Singleton).AsSelf();

        // SoundController の登録
        builder.RegisterEntryPoint<SoundController>(Lifetime.Singleton).AsSelf();

        // TextModel の登録
        builder.Register<TextModel>(Lifetime.Singleton);

        // TextController の登録
        builder.RegisterEntryPoint<TextController>(Lifetime.Singleton).AsSelf();

        // FadeService の登録
        builder.Register<IFadeService, FadeService>(Lifetime.Singleton);

        // SceneService の登録
        builder.Register<ISceneService, SceneService>(Lifetime.Singleton);

        // SoundService の登録
        builder.Register<ISoundService, SoundService>(Lifetime.Singleton);

        // MouseInput の登録
        builder.Register<IMouseInput, MouseInput>(Lifetime.Singleton);
    }
}
