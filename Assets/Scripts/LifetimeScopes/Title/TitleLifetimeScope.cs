using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class TitleLifetimeScope : LifetimeScope
{
    [SerializeField] private TitleView titleView;

    protected override void Configure(IContainerBuilder builder)
    {
        // ===== タイトル画面システム =====

        // TitleController の登録
        builder.RegisterEntryPoint<TitleController>(Lifetime.Singleton).AsSelf();

        // TitleView の登録
        builder.RegisterComponent(titleView);
    }
}
