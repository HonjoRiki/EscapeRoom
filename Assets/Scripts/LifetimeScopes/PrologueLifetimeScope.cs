using VContainer;
using VContainer.Unity;

public class PrologueLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<PrologueController>(Lifetime.Singleton).AsSelf();

        // PlayerInput登録
        builder.Register<PlayerInput>(Lifetime.Singleton).AsImplementedInterfaces();
    }
}
