using Cysharp.Threading.Tasks;
using VContainer.Unity;

public class MainGameController : IStartable
{
    private readonly IFadeService fadeService;

    public MainGameController(IFadeService fadeService)
    {
        this.fadeService = fadeService;
    }

    public void Start()
    {
        fadeService.FadeInAsync(1.0f).Forget();
    }
}
