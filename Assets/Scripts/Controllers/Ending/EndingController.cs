using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer.Unity;

public sealed class EndingController : IStartable
{
    private readonly EndingView view;
    private readonly IFadeService fadeService;
    private readonly CompositeDisposable disposable = new();

    public EndingController(EndingView view, IFadeService fadeService)
    {
        this.view = view;
        this.fadeService = fadeService;
    }

    public void Start()
    {
        view.OnSurveyButtonClicked
            .Subscribe(_ => OpenSurveyPage())
            .AddTo(disposable);

        fadeService.FadeInAsync(1.0f).Forget();
    }

    private void OpenSurveyPage()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSc3sHcBpGcO_BB0vWFHA5qo158X60W6wrWTueEuv4VA8EB58A/viewform?usp=publish-editor");
    }
}
