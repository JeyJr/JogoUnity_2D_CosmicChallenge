using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SFXClip {btn, getItem, hit, startMatch}
public class SFXControl : MonoBehaviour, ISetAudioVolume
{
    private readonly string sfxKey = "sfxVolume";

    [SerializeField] private List<AudioClip> clipList;
    [SerializeField] private AudioSource audioSource;
    private Dictionary<SFXClip, AudioClip> clips;



    private void Start()
    {
        clips = new Dictionary<SFXClip, AudioClip>();
        clips.Add(SFXClip.btn, clipList[(int)SFXClip.btn]);
        clips.Add(SFXClip.getItem, clipList[(int)SFXClip.getItem]);
        clips.Add(SFXClip.hit, clipList[(int)SFXClip.hit]);
        clips.Add(SFXClip.startMatch, clipList[(int)SFXClip.startMatch]);

        LoadVolume();

        GameEvent.GetInstance().OnSFXVolumeChange += SetVolume;
        GameEvent.GetInstance().OnPlaySFXClip += PlaySFXClip;
    }

    public void LoadVolume()
    {
        if (!PlayerPrefs.HasKey(sfxKey))
        {
            PlayerPrefs.SetFloat(sfxKey, .5f);
        }

        audioSource.volume = PlayerPrefs.GetFloat(sfxKey);
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(sfxKey, volume);
    }

    public void PlaySFXClip(SFXClip clip)
    {
        if (clips.ContainsKey(clip))
        {
            audioSource.PlayOneShot(clips[clip]);
        }
    }
}
