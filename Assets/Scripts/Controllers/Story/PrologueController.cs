using Cysharp.Threading.Tasks;
using R3;
using VContainer.Unity;

public sealed class PrologueController : IStartable
{
    private readonly IFadeService fadeService;
    private readonly ISceneService sceneService;
    private readonly TextModel textModel;
    private readonly IPlayerInput playerInput;


    public PrologueController(
        IFadeService fadeService,
        ISceneService sceneService,
        TextModel textModel,
        IPlayerInput playerInput)
    {
        this.fadeService = fadeService;
        this.sceneService = sceneService;
        this.textModel = textModel;
        this.playerInput = playerInput;
    }

    public void Start()
    {
        PlayPrologueSequence().Forget();
    }

    private async UniTask PlayPrologueSequence()
    {
        // プロローグシーケンスの実装
        await UniTask.Delay(2000); // 仮の遅延

        textModel.SetText("ここは見知らぬ部屋……");
        await playerInput.OnInteract.FirstAsync().AsUniTask();
        textModel.SetText("どうやら脱出しなければならないようだ。");
        await playerInput.OnInteract.FirstAsync().AsUniTask();
        textModel.SetText("周りを見渡してみよう。");
        await playerInput.OnInteract.FirstAsync().AsUniTask();
        textModel.SetText("なにか手がかりがあるかもしれない。");
        await playerInput.OnInteract.FirstAsync().AsUniTask();
        textModel.ClearText();

        // フェードアウト
        await fadeService.FadeOutAsync(1.0f);

        // シーン遷移
        await sceneService.LoadSceneAsync("Stage1");
    }
}
