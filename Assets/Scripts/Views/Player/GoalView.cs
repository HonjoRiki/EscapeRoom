using UnityEngine;

public sealed class GoalView : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    // 規約に則り、読み取り専用プロパティは => 構文で公開
    public string NextSceneName => nextSceneName;
}