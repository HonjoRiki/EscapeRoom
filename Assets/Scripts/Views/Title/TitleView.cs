using R3;
using UnityEngine;
using UnityEngine.UI;

public sealed class TitleView : MonoBehaviour
{
    [SerializeField] private Button startButton;

    public Observable<Unit> OnStartButtonClicked => startButton.OnClickAsObservable();
}
