using UnityEngine;
using VContainer.Unity;
using R3;

public sealed class SoundController : IStartable
{
    private readonly ISoundService soundService;
    private readonly SoundView soundView;

    public SoundController(ISoundService soundService, SoundView soundView)
    {
        this.soundService = soundService;
        this.soundView = soundView;
    }

    public void Start()
    {
        soundService.BackgroundMusicPlayed.Subscribe(clip =>
        {
            soundView.PlayBackgroundMusic(clip);
        });
    }
}
