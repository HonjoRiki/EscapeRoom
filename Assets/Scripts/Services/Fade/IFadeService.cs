using Cysharp.Threading.Tasks;

public interface IFadeService
{
    UniTask FadeInAsync(float duration);
    UniTask FadeOutAsync(float duration);
}