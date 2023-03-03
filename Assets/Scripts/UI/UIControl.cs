using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Panels { MainMenu, Difficulty, Config, HUD, EndGame}
public class UIControl : MonoBehaviour
{
    [SerializeField] private GameObject[] panelsGameObj;
    [SerializeField] private Dictionary<Panels, GameObject> panels = new Dictionary<Panels, GameObject>();

    [Header("MAinMenu")]
    [SerializeField] private TextMeshProUGUI txtRecordMainMenu;


    [Header("ENDGAME")]
    [SerializeField] private TextMeshProUGUI txtCurrentTime;
    [SerializeField] private TextMeshProUGUI txtCollectedItems;
    [SerializeField] private TextMeshProUGUI txtGameMode;
    [SerializeField] private TextMeshProUGUI txtTimeRecord;

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI txtDurationMatch;
    [SerializeField] private TextMeshProUGUI txtInitialMessege;
    private TimeSpan durationMatch = TimeSpan.Zero;
    private TimeSpan timeLimitToEndMatch = TimeSpan.FromMinutes(10);


    private void Awake()
    {
        AddPanelsToDictionary();
    }
    private void Start()
    {
        GameEvent.GetInstance().OnStartMatch += SetHUDBehavior; //Exibir mensagem
        GameEvent.GetInstance().OnStartSpawn += SetStartTimeCount; //Iniciar contagem de tempo junto com spawns
        
        GameEvent.GetInstance().OnMainMenu += SetMainMenuBehavior;
        GameEvent.GetInstance().OnEndGame += SetEndGameBehavior;

        BtnRestartMainMenu();
    }
    private void AddPanelsToDictionary()
    {
        for (int i = 0; i < panelsGameObj.Length; i++)
        {
            Panels p = (Panels)i;

            if (panels.ContainsKey(p))
            {
                continue;
            }
            panels.Add(p, panelsGameObj[i]);
        }
    } 
    public void EnablePanel(Panels panel)
    {
        foreach (var item in panels)
        {
            if(item.Key == panel)
            {
                item.Value.SetActive(true);
            }
            else
            {
                item.Value.SetActive(false);
            }
        }
    }
    private void SetMainMenuBehavior()
    {
        EnablePanel(Panels.MainMenu);
        txtRecordMainMenu.text = GameController.GetInstance().CheckNewRecord();
    }

    #region SFX: PlayBtnSoundClip, PlayBtnStartMatchSoundClip
    public void PlayBtnSoundClip()
    {
        GameEvent.GetInstance().PlaySFXClip(SFXClip.btn);
    }
    public void PlayBtnStartMatchSoundClip()
    {
        GameEvent.GetInstance().PlaySFXClip(SFXClip.startMatch);
    }
    #endregion

    #region Buttons: Open-Close Panels
    public void BtnRestartMainMenu() => GameController.GetInstance().SetGameState(GameState.MainMenu); //No endGame, voltar para mainMenu
    public void BtnBackMainMenu()
    {
        EnablePanel(Panels.MainMenu);
        txtRecordMainMenu.text = GameController.GetInstance().CheckNewRecord();
    } //no MainMenu mas em outros paineis

    public void BtnOpenDifficulty() => EnablePanel(Panels.Difficulty);
    public void BtnOpenConfig() => EnablePanel(Panels.Config);
    public void BtnBackPanel(GameObject gameObject) => gameObject.SetActive(!gameObject.activeSelf); 
    public void BtnStartMatch() 
    {
        GameController.GetInstance().SetGameState(GameState.StartMatch);
        EnablePanel(Panels.HUD);
    }
    #endregion


    #region HUD: SetHUDBehavior, UpdateTextTimeCount
    /// <summary>
    /// HUD behavior controla a contagem de tempo e a exibição do mesmo na tela
    /// Ela armazena o tempo corrido para que possa ser comparado com o recorde do player
    /// </summary>

    private void SetHUDBehavior()
    {
        EnablePanel(Panels.HUD);

        txtDurationMatch.text = "00:00";
        StartCoroutine(InitialMessege()); //Exibir mensagem inicial
    }

    private void SetStartTimeCount() => StartCoroutine(StartTimeCount()); //Iniciar contagem de tempo

    IEnumerator InitialMessege()
    {
        txtInitialMessege.text = string.Format("Colete <color=red> {0} </color> \r\njoias espaciais!", GameController.GetInstance().MaxItems);
        txtInitialMessege.gameObject.SetActive(true);

        float delay = GameController.GetInstance().DelayToStartSpawn;
        yield return new WaitForSeconds(delay);

        txtInitialMessege.gameObject.SetActive(false);
    }

    IEnumerator StartTimeCount()
    {

        DateTime startTime = DateTime.Now;

        while(GameController.GetInstance().GameState == GameState.StartMatch)
        {
            yield return null;
            durationMatch = DateTime.Now - startTime;
            txtDurationMatch.text = string.Format("{0:00}:{1:00}", durationMatch.Minutes, durationMatch.Seconds);
            GameController.GetInstance().SetDurationMatch(durationMatch);

            if(durationMatch >= timeLimitToEndMatch)
            {
                GameController.GetInstance().SetGameState(GameState.EndGame);
                break;
            }
        }
    }

    #endregion

    #region EndGameUI: SetEndGameBehavior, TextEndGame
    /// <summary>
    /// EndGameUI responsavel por controlar a exibição das informações na ui de endGame
    /// A mesma é responsavel por verificar se existe um novo recorde
    /// </summary>

    private void SetEndGameBehavior()
    {
        EnablePanel(Panels.EndGame);
        TextEndGame();
    }

    private void TextEndGame()
    {
        txtCurrentTime.text = "<color=#31CFEF>Tempo:</color> " + GameController.GetInstance().GetDurationMatch();
        txtGameMode.text = "<color=#31CFEF>Mode:</color>" + GameController.GetInstance().GetGameModeName();
        txtCollectedItems.text = $"x {GameController.GetInstance().CollectedItems}";
        
        txtTimeRecord.text = GameController.GetInstance().CheckNewRecord();
    }

    #endregion
}
