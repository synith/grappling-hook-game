using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public float Volume { get; private set; } = 0.5f;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        Volume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        audioSource.volume = Volume;
    }


    public void IncreaseVolume()
    {
        Volume += 0.1f;
        Volume = Mathf.Clamp01(Volume);
        audioSource.volume = Volume;
        PlayerPrefs.SetFloat("musicVolume", Volume);
    }


    public void DecreaseVolume()
    {
        Volume -= 0.1f;
        Volume = Mathf.Clamp01(Volume);
        audioSource.volume = Volume;
        PlayerPrefs.SetFloat("musicVolume", Volume);
    }
}
