using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using VContainer.Unity;

public sealed class PlayerInteractionController : IStartable, IDisposable
{
    private readonly IPlayerInput playerInput;
    private readonly PlayerView playerView;
    private readonly TextModel textModel;
    private readonly ISceneService sceneService;
    private readonly PlayerModel playerModel;
    private readonly IFadeService fadeService;
    private readonly CompositeDisposable disposable = new();

    private bool isInteracting = false;

    public PlayerInteractionController(
        IPlayerInput playerInput,
        PlayerView playerView,
        TextModel textModel,
        ISceneService sceneService,
        PlayerModel playerModel,
        IFadeService fadeService)
    {
        this.playerInput = playerInput;
        this.playerView = playerView;
        this.textModel = textModel;
        this.sceneService = sceneService;
        this.playerModel = playerModel;
        this.fadeService = fadeService;
    }

    public void Start()
    {
        // 入力イベントを購読し、Interactメソッドを呼び出す
        playerInput.OnInteract
            .Subscribe(_ => Interact().Forget())
            .AddTo(disposable);
    }

    private async UniTask Interact()
    {
        if (isInteracting) return;
        isInteracting = true;
        playerModel.IsInputActive = false;

        var goalView = playerView.GetCurrentTriggers()
            .Select(c => c.GetComponent<GoalView>())
            .FirstOrDefault(g => g != null);

        if (goalView != null)
        {
            await HandleGoalInteraction(goalView);
        }
        else
        {
            await HandleNoInteraction();
        }

        playerModel.IsInputActive = true;
        isInteracting = false;
    }

    private async UniTask HandleGoalInteraction(GoalView goalView)
    {
        textModel.SetText("なにかある……");
        await playerInput.OnInteract.FirstAsync().AsUniTask();

        textModel.SetText("ボタンを見つけた。押してみる……");
        await playerInput.OnInteract.FirstAsync().AsUniTask();

        textModel.SetText("扉が開いた！");
        await playerInput.OnInteract.FirstAsync().AsUniTask();

        textModel.ClearText();
        // シーン遷移の前に1秒かけてフェードアウト
        await fadeService.FadeOutAsync(1.0f);
        await sceneService.LoadSceneAsync(goalView.NextSceneName);
    }

    private async UniTask HandleNoInteraction()
    {
        textModel.SetText("ここには何もないみたい……");
        await playerInput.OnInteract.FirstAsync().AsUniTask();
        textModel.ClearText();
    }

    public void Dispose()
    {
        disposable.Dispose();
    }
}