using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public sealed class SceneService : ISceneService
{
    public async UniTask LoadSceneAsync(string sceneName)
    {
        // UniTaskを使って非同期にシーンをロード
        await SceneManager.LoadSceneAsync(sceneName).ToUniTask();
    }
}