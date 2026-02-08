using UnityEngine;

public sealed class SoundView : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusicSource;

    public void PlayBackgroundMusic(AudioClip clip, bool loop = true)
    {
        backgroundMusicSource.clip = clip;
        backgroundMusicSource.loop = loop;
        backgroundMusicSource.Play();
    }
}
