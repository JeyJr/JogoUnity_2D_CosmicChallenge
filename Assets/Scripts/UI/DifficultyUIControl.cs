using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Classe atribuida ao panel Difficulty, onde os metodos são atribuidos diretamente no OnClick dos Componentes Button
/// Responsavel por controlar a dificuldade do jogo
/// </summary>
public class DifficultyUIControl : MonoBehaviour
{
    [SerializeField] private Slider sliderGameMode;
    [SerializeField] private TextMeshProUGUI txtGameMode;

    private void OnEnable()
    {
        UpdateDifficultyInfo();
    }

    public void BtnResetRecord() 
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }

    public void BtnAdd()
    {
        GameController.GetInstance().AddGameModeValue();
        UpdateDifficultyInfo();
    }

    public void BtnSub()
    {
        GameController.GetInstance().SubGameModeValue();
        UpdateDifficultyInfo();
    }

    private void UpdateDifficultyInfo()
    {
        sliderGameMode.minValue = 0;
        sliderGameMode.maxValue = GameController.GetInstance().GetGameModeLength() - 1;
        sliderGameMode.value = GameController.GetInstance().GetGameModeValue();
        txtGameMode.text = "Modo > " + GameController.GetInstance().GetGameModeName();
    }
}
