using Cysharp.Threading.Tasks;
using VContainer.Unity;
using R3;
using UnityEngine;

public sealed class TitleController : IStartable
{
    private readonly ISceneService sceneService;
    private readonly IFadeService fadeService;
    private readonly ISoundService soundService;
    private readonly TitleView view;

    public TitleController(
        ISceneService sceneService,
        IFadeService fadeService,
        ISoundService soundService,
        TitleView view)
    {
        this.sceneService = sceneService;
        this.fadeService = fadeService;
        this.soundService = soundService;
        this.view = view;
    }

    public void Start()
    {
        soundService.PlayBackgroundMusic(view.BackgroundMusicClip);
        view.OnStartButtonClicked.Subscribe(_ => StartGame().Forget());
        fadeService.FadeInAsync(1.0f).Forget();
    }

    private async UniTask StartGame()
    {
        await fadeService.FadeOutAsync(1.0f);
        await sceneService.LoadSceneAsync("Prologue");
    }
}
