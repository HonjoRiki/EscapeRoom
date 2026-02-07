using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // ===== 共通システム (インゲーム) =====

        // GameController の登録
        builder.RegisterEntryPoint<MainGameController>(Lifetime.Singleton).AsSelf();
    }
}
