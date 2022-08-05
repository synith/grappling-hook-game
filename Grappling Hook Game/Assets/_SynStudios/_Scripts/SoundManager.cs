using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public enum Sound
    {
        GrappleShoot,
        GrappleShotFlying,
        GrappleHit,
        GrappleSwinging,
        GrappleSwinging_2,
        GrappleRelease,
        Landing,
        GameWon,
        ButtonPress,
        Jump,
        Pause,
        Unpause,
        Collected,
    }
    public float Volume { get; private set; } = 0.5f;

    private AudioSource audioSource;
    private Dictionary<Sound, AudioClip> soundAudioClipDictionary;

    private const string SOUND_VOLUME = "soundVolume";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Volume = PlayerPrefs.GetFloat(SOUND_VOLUME, 0.5f);

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = Volume;

        soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();

        foreach (Sound sound in System.Enum.GetValues(typeof(Sound)))
        {
            soundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }

    public void PlaySound(Sound sound)
    {
        audioSource.PlayOneShot(soundAudioClipDictionary[sound], Volume);
    }
    public void IncreaseVolume()
    {
        Volume += 0.1f;
        Volume = Mathf.Clamp01(Volume);
        PlayerPrefs.SetFloat(SOUND_VOLUME, Volume);
    }
    public void DecreaseVolume()
    {
        Volume -= 0.1f;
        Volume = Mathf.Clamp01(Volume);
        PlayerPrefs.SetFloat(SOUND_VOLUME, Volume);
    }

}
