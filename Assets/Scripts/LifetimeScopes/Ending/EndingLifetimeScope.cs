using UnityEngine;
using VContainer;
using VContainer.Unity;

public class EndingLifetimeScope : LifetimeScope
{
    [SerializeField] private EndingView endingView;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(endingView);
        builder.RegisterEntryPoint<EndingController>(Lifetime.Singleton).AsSelf();
    }
}
