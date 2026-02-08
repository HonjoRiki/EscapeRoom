using R3;
using UnityEngine;
using UnityEngine.UI;

public sealed class TitleView : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private AudioClip backgroundMusicClip;

    public Observable<Unit> OnStartButtonClicked => startButton.OnClickAsObservable();

    public AudioClip BackgroundMusicClip => backgroundMusicClip;
}
