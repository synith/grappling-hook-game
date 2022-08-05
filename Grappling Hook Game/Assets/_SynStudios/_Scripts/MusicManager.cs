using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    public enum Music
    {
        Menu,
        Game,
    }
    public float Volume { get; private set; } = 0.5f;

    private AudioSource audioSource;
    private Dictionary<Music, AudioClip> musicAudioClipDictionary;

    private const string MUSIC_VOLUME = "musicVolume";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Volume = PlayerPrefs.GetFloat(MUSIC_VOLUME, 0.5f);

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = Volume;

        musicAudioClipDictionary = new Dictionary<Music, AudioClip>();

        foreach (Music music in Enum.GetValues(typeof(Music)))
        {
            musicAudioClipDictionary[music] = Resources.Load<AudioClip>(music.ToString());
        }
    }

    public void PlayMusic(Music music)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        print($"Music Playing: {music}");
        audioSource.clip = musicAudioClipDictionary[music];

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void IncreaseVolume()
    {
        Volume += 0.1f;
        Volume = Mathf.Clamp01(Volume);
        audioSource.volume = Volume;
        PlayerPrefs.SetFloat(MUSIC_VOLUME, Volume);
    }


    public void DecreaseVolume()
    {
        Volume -= 0.1f;
        Volume = Mathf.Clamp01(Volume);
        audioSource.volume = Volume;
        PlayerPrefs.SetFloat(MUSIC_VOLUME, Volume);
    }
}
