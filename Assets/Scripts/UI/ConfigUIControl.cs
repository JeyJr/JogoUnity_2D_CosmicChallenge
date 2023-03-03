using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe atribuida ao Canvas > PanelConfig 
/// Responsavel por disponibilizar para os botões no panelConfig o controle do volume 
/// </summary>
public class ConfigUIControl : MonoBehaviour
{
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private TextMeshProUGUI txtMusic;

    [SerializeField] private Slider sliderSFX;
    [SerializeField] private TextMeshProUGUI txtSFX;

    private void OnEnable()
    {
        sliderMusic.value = PlayerPrefs.GetFloat("musicVolume");
        sliderSFX.value = PlayerPrefs.GetFloat("sfxVolume");
        UpdateMusicVolumeInfo();
    }

    #region MusicVolume: BtnAddMusicVolume, BtnSubMusicVolume, UpdateMusicVolumeInfo
    public void BtnAddMusicVolume()
    {
        sliderMusic.value += .1f;
        UpdateMusicVolumeInfo();
        PlaySoundClip();
        GameEvent.GetInstance().MusicVolumeChange(sliderMusic.value);
    }

    public void BtnSubMusicVolume()
    {
        sliderMusic.value-= .1f;
        UpdateMusicVolumeInfo();
        PlaySoundClip();
        GameEvent.GetInstance().MusicVolumeChange(sliderMusic.value);
    }

    private void UpdateMusicVolumeInfo()
    {
        sliderMusic.minValue = 0;
        sliderMusic.maxValue = 1;
        txtMusic.text = "Music: " + string.Format("{0:00}", sliderMusic.value * 100) + "%";
    }
    #endregion

    #region SFXVolume: BtnAddSFXVolume, BtnSubSFXVolume, UpdateSFXVolumeInfo
    public void BtnAddSFXVolume()
    {
        sliderSFX.value += .1f;
        UpdateSFXVolumeInfo();
        PlaySoundClip();

        GameEvent.GetInstance().SFXVolumeChange(sliderSFX.value);
    }

    public void BtnSubSFXVolume()
    {
        sliderSFX.value -= .1f;
        UpdateSFXVolumeInfo();
        PlaySoundClip();

        GameEvent.GetInstance().SFXVolumeChange(sliderSFX.value);
    }

    private void UpdateSFXVolumeInfo()
    {
        sliderSFX.minValue = 0;
        sliderSFX.maxValue = 1;
        txtSFX.text = "SFX: " + string.Format("{0:00}", sliderSFX.value * 100) + "%";
    }

    private void PlaySoundClip()
    {
        GameEvent.GetInstance().PlaySFXClip(SFXClip.btn);
    }
    #endregion
}
