using System;
using R3;
using VContainer.Unity;

public sealed class TextController : IStartable, IDisposable
{
    private readonly TextModel model;
    private readonly TextView view;
    private readonly CompositeDisposable disposable = new();

    public TextController(TextModel model, TextView view)
    {
        this.model = model;
        this.view = view;
    }

    public void Start()
    {
        // Modelの変更を監視し、Viewのメソッドを呼び出して見た目に反映する
        model.CurrentText
            .Subscribe(view.SetText)
            .AddTo(disposable);

        model.IsActive
            .Subscribe(view.SetActive)
            .AddTo(disposable);
    }

    public void Dispose()
    {
        disposable.Dispose();
    }
}
