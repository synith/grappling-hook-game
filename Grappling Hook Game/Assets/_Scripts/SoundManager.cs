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
    }
    public float Volume { get; private set; } = 0.5f;

    private AudioSource audioSource;
    private Dictionary<Sound, AudioClip> audioClipDictionary;
    


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Volume = PlayerPrefs.GetFloat("soundVolume", 0.5f);

        audioSource = GetComponent<AudioSource>();
        audioClipDictionary = new Dictionary<Sound, AudioClip>();

        foreach (Sound sound in System.Enum.GetValues(typeof(Sound)))
        {
            audioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }

    public void PlaySound(Sound sound)
    {
        audioSource.PlayOneShot(audioClipDictionary[sound], Volume);
    }
    public void IncreaseVolume()
    {
        Volume += 0.1f;
        Volume = Mathf.Clamp01(Volume);
        PlayerPrefs.SetFloat("soundVolume", Volume);
    }
    public void DecreaseVolume()
    {
        Volume -= 0.1f;
        Volume = Mathf.Clamp01(Volume);
        PlayerPrefs.SetFloat("soundVolume", Volume);
    }

}
