using Cysharp.Threading.Tasks;

public interface ISceneService
{
    UniTask LoadSceneAsync(string sceneName);
}