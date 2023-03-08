using System;
using UnityEngine;

public class GameEvent : MonoBehaviour 
{
    private static GameEvent instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static GameEvent GetInstance()
    {
        return instance;
    }

    public event Action OnMainMenu;
    public event Action OnStartMatch;
    public event Action OnEndGame;

    public event Action OnStartSpawn;

    public event Action OnCollectItems;
    public event Action OnRemoveItems;

    public event Action<float> OnMusicVolumeChange;
    public event Action<float> OnSFXVolumeChange;
    public event Action<SFXClip> OnPlaySFXClip;

    public void StartMatch()
    {
        OnStartMatch?.Invoke();
    }
    public void StartSpawn()
    {
        OnStartSpawn?.Invoke();
    }
    public void EndGame()
    {
        OnEndGame?.Invoke();
    }
    
    public void MainMenu()
    {
        OnMainMenu?.Invoke();
    }
    public void CollectItems()
    {
        OnCollectItems?.Invoke();
    }
    public void RemoveItems()
    {
        OnRemoveItems?.Invoke();
    }
    public void MusicVolumeChange(float value)
    {
        OnMusicVolumeChange?.Invoke(value);
    }
    public void SFXVolumeChange(float value)
    {
        OnSFXVolumeChange?.Invoke(value);
    }
    public void PlaySFXClip(SFXClip clip)
    {
        OnPlaySFXClip?.Invoke(clip);
    }
}

