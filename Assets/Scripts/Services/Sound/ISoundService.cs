using R3;
using UnityEngine;

public interface ISoundService
{
    Observable<AudioClip> BackgroundMusicPlayed { get; }
    void PlayBackgroundMusic(AudioClip clip, bool loop = true);
}
