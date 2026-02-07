using R3;

public sealed class FadeModel
{
    // 初期値は透明(alpha=0)
    private readonly ReactiveProperty<float> alpha = new ReactiveProperty<float>(0f);
    public ReadOnlyReactiveProperty<float> Alpha => alpha;

    public void SetAlpha(float value)
    {
        alpha.Value = value;
    }
}