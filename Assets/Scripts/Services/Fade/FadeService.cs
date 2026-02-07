using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// フェード処理のロジックを担当するサービスクラス
/// </summary>
public sealed class FadeService : IFadeService
{
    private readonly FadeModel model;

    public FadeService(FadeModel model)
    {
        this.model = model;
    }

    public async UniTask FadeInAsync(float duration)
    {
        await this.FadeAsync(1f, 0f, duration);
    }

    public async UniTask FadeOutAsync(float duration)
    {
        await this.FadeAsync(0f, 1f, duration);
    }

    private async UniTask FadeAsync(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            var newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            this.model.SetAlpha(newAlpha);
            await UniTask.Yield();
        }
        this.model.SetAlpha(endAlpha);
    }
}