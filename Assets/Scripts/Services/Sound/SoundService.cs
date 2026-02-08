using R3;
using UnityEngine;

public sealed class SoundService : ISoundService
{
    public Observable<AudioClip> BackgroundMusicPlayed => backgroundMusicPlayed;
    private readonly Subject<AudioClip> backgroundMusicPlayed = new();

    public void PlayBackgroundMusic(AudioClip clip, bool loop = true)
    {
        backgroundMusicPlayed.OnNext(clip);
    }
}
