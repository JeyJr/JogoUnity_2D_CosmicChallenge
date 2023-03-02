using System;
using UnityEngine;


public class GameData : MonoBehaviour
{
    [Header("GameMode")]
    [SerializeField] private int gameModeValue = 4;
    [SerializeField] private string[] gameModeName = new[]
    {
        "Muito Fácil",
        "Fácil",
        "Normal",
        "Humano",
        "Difícil",
        "Muito Difícil",
        "Insano",
        "Alienígena",
        "Astronômico",
        "Deus"
    };
    public string GameModeName => gameModeName[gameModeValue];


    [Header("BoxControl")]
    [SerializeField] private int collectedItems = 0;
    [SerializeField] private int maxItems = 1;
    public int CollectedItems => collectedItems;

    [Header("TimeControl")]
    [SerializeField] private TimeSpan recordTime;
    [SerializeField] private TimeSpan durationOfMatch;
    public TimeSpan RecordTime => recordTime;

    private readonly string recordKey = "record";
    


    private void Start()
    {
        CheckSavedRecord();
        SetItemsEvent();
    }

    #region Box: AddCollectedItems, AddCollectedItems, RemoveCollectedItems

    private void SetItemsEvent()
    {
        GameEvent.GetInstance().OnStartMatch += ResetCollectedItems;
        GameEvent.GetInstance().OnCollectItems += AddCollectedItems;
        GameEvent.GetInstance().OnRemoveItems += RemoveCollectedItems;
    }

    private void AddCollectedItems()
    {
        collectedItems++;

        if(collectedItems >= maxItems)
        {
            GameController.GetInstance().SetGameState(GameState.EndGame);
        }
    }

    private void RemoveCollectedItems()
    {
        if(collectedItems > 0)
        {
            collectedItems--;
        }
    }

    private void ResetCollectedItems()
    {
        collectedItems = 0;
    }
    #endregion

    #region Record: CheckSavedRecord, CheckNewRecord, SetDurationMatch, GetDurationMatch, FormatTimeSpan
    private void CheckSavedRecord()
    {
        if (!PlayerPrefs.HasKey(recordKey))
        {
            PlayerPrefs.SetString(recordKey, TimeSpan.Zero.ToString());
        }

        recordTime = TimeSpan.Parse(PlayerPrefs.GetString(recordKey));
    }

    public string CheckNewRecord()
    {
        if(collectedItems >= maxItems)
        {
            if (recordTime <= TimeSpan.Zero || durationOfMatch < recordTime)
            {
                //new Recorde
                PlayerPrefs.SetString(recordKey, durationOfMatch.ToString());
                return "Novo recorde: " + FormatTimeSpan(durationOfMatch);
            }
        }

        return "Recorde: " + FormatTimeSpan(recordTime);
    }


    public void SetDurationMatch(TimeSpan duration)
    {
        durationOfMatch = duration;
    }

    public string GetDurationMatch()
    {
        return FormatTimeSpan(durationOfMatch);
    }

    private string FormatTimeSpan(TimeSpan time)
    {
        return string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
    }

    #endregion

    public float GetMoveSpeed(float currentValue)
    {
        float gameMode = gameModeValue + 1;
        return currentValue + (gameMode * .3f);
    }

    public float GetSpawnSpeed(float currentValue) 
    {
        float gameMode = gameModeValue + 1;
        return currentValue + (gameMode * .2f);
    }

}
