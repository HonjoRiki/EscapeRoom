using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer.Unity;

/// <summary>
/// FadeModelとFadeViewを接続するコントローラー
/// </summary>
public sealed class FadeController : IStartable, IDisposable
{
    private readonly FadeModel model;
    private readonly FadeView view;
    private readonly CompositeDisposable disposable = new CompositeDisposable();

    public FadeController(FadeModel model, FadeView view)
    {
        this.model = model;
        this.view = view;
    }

    public void Start()
    {
        // ModelのAlpha値の変更を監視し、ViewのSetAlphaを呼び出す
        this.model.Alpha
            .Subscribe(this.view.SetAlpha)
            .AddTo(this.disposable);
    }

    public void Dispose()
    {
        this.disposable.Dispose();
    }
}