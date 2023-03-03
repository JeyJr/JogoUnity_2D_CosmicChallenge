using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour, ISetAudioVolume
{
    [SerializeField] private List<AudioClip> musicClipList;
    [SerializeField] private AudioSource audioSource;
    private int currentMusicIndex;
    private readonly string volumeKey = "musicVolume";

    private void Start()
    {
        LoadVolume();
        PlayRandomMusic();

        GameEvent.GetInstance().OnMusicVolumeChange += SetVolume;
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayRandomMusic();
        }
    }


    private void PlayRandomMusic()
    {
        currentMusicIndex = Random.Range(0, musicClipList.Count);
        audioSource.clip = musicClipList[currentMusicIndex];
        audioSource.Play();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(volumeKey, volume);
    }

    public void LoadVolume()
    {
        if (!PlayerPrefs.HasKey(volumeKey))
        {
            PlayerPrefs.SetFloat(volumeKey, .5f);
        }

        SetVolume(PlayerPrefs.GetFloat(volumeKey));
    }
}
